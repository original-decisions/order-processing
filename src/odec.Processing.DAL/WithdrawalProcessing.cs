using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Microsoft.EntityFrameworkCore;
#if net452
using System.Transactions;
#endif
using odec.Framework.Extensions;
using odec.Framework.Logging;
using odec.Processing.DAL.Interop;
using odec.Server.Model.OrderProcessing;
using odec.Server.Model.OrderProcessing.Filters;
using odec.Server.Model.OrderProcessing.Money;
using odec.Server.Model.OrderProcessing.Money.Withdrawal;
using odec.Server.Model.User;

namespace odec.Processing.DAL
{
    public class WithdrawalProcessing:
        IWithdrawalProcessing<int,DbContext,WithdrawalApplication,WithdrawalMethod,User, WithdrawalApplicationFilter<int, int?>>
    {
        SystemMoneyProcessing repository = new SystemMoneyProcessing();
        public WithdrawalProcessing()
        {
            
        }
        public WithdrawalProcessing(DbContext db)
        {
            Db = db;
            repository.SetContext(db);
        }
        public DbContext Db { get; set; }
        public void SetConnection(string connection)
        {
            throw new NotImplementedException();
        }

        public void SetContext(DbContext db)
        {
            Db = db;
            repository.SetContext(db);
        }

        public WithdrawalApplication GetById(int id)
        {
            try
            {
                return Db.Set<WithdrawalApplication>().SingleOrDefault(it => it.Id==id);
            }
            catch (Exception ex)
            {
                LogEventManager.Logger.Error(ex.Message, ex);
                throw;
            }
        }

        public WithdrawalApplication CreateApplication(int withdrawalMethodId, int userId, DateTime withdrawalBefore, decimal amount, string comment)
        {
            try
            {
                var withdrawalApp = new WithdrawalApplication
                {
                    UserId = userId,
                    WithdrawalMethodId = withdrawalMethodId,
                    DateCreated = DateTime.Now,
                    WithdrawalBefore = withdrawalBefore,
                    Amount = amount,
                    Comment = comment ?? String.Empty
                };
                Db.Set<WithdrawalApplication>().Add(withdrawalApp);
                Db.SaveChanges();
                return withdrawalApp;
            }
            catch (Exception ex)
            {
                LogEventManager.Logger.Error(ex.Message, ex);
                throw;
            }
        }

        public WithdrawalApplication CreateApplication(WithdrawalMethod withdrawalMethod, User user, DateTime withdrawalBefore, decimal amount,
            string comment)
        {
            try
            {
                return CreateApplication(withdrawalMethod.Id, user.Id, withdrawalBefore, amount, comment);
            }
            catch (Exception ex)
            {
                LogEventManager.Logger.Error(ex.Message, ex);
                throw;
            }
        }

        public WithdrawalApplication CreateApplication(WithdrawalApplication application)
        {
            try
            {
                Db.Set<WithdrawalApplication>().Add(application);
                Db.SaveChanges();
                return application;
            }
            catch (Exception ex)
            {
                LogEventManager.Logger.Error(ex.Message, ex);
                throw;
            }
        }

        public IEnumerable<WithdrawalApplication> GetApplications(WithdrawalApplicationFilter<int,int?> filter)
        {
            try
            {
                var query = Db.Set<WithdrawalApplication>().Where(it => it.UserId ==filter.UserId);
                if (filter.WithdrawalMethodId.HasValue)
                    query = query.Where(it => it.WithdrawalMethodId == filter.WithdrawalMethodId);

                //date created
                if (filter.DateCreatedInterval != null)
                {
                    if (filter.DateCreatedInterval.Start != null)
                        query = query.Where(it => it.DateCreated >= filter.DateCreatedInterval.Start);
                    if (filter.DateCreatedInterval.End != null)
                        query = query.Where(it => it.DateCreated <= filter.DateCreatedInterval.End);
                }
                //date Withdrawal Before 
                if (filter.WithdrawalBeforeInterval != null)
                {
                    if (filter.WithdrawalBeforeInterval.Start != null)
                        query = query.Where(it => it.WithdrawalBefore >= filter.WithdrawalBeforeInterval.Start);
                    if (filter.WithdrawalBeforeInterval.End != null)
                        query = query.Where(it => it.WithdrawalBefore <= filter.WithdrawalBeforeInterval.End);
                }
                if (filter.Amount != null)
                {
                    if (filter.Amount.Start != null)
                        query = query.Where(it => it.Amount >= filter.Amount.Start);
                    if (filter.Amount.End != null)
                        query = query.Where(it => it.Amount <= filter.Amount.End);
                }
                if (!string.IsNullOrEmpty(filter.Sidx))
                    query = filter.Sord.Equals("desc", StringComparison.OrdinalIgnoreCase)
                    ? query.OrderByDescending(filter.Sidx)
                    : query.OrderBy(filter.Sidx);
                if (filter.Page != 0 && filter.Rows != 0)
                    return query.Skip(filter.Rows * (filter.Page - 1)).Take(filter.Rows);
                return query;
            }
            catch (Exception ex)
            {
                LogEventManager.Logger.Error(ex.Message, ex);
                throw;
            }
        }

        public void ApproveWithdrawal(WithdrawalApplication application)
        {
            try
            {
#if net452
                using (var scope = new TransactionScope(TransactionScopeOption.Required,new TransactionOptions
                {
                    Timeout = TimeSpan.FromMinutes(5),
                    IsolationLevel = IsolationLevel.ReadCommitted
                }))
                {
#endif
                    var withdrawalApplication = Db.Set<WithdrawalApplication>().Single(it => it.Id == application.Id);
                    if (withdrawalApplication.UserId != application.UserId)
                        throw new ArgumentException("User vialotion. Access is denied to operation");

                    withdrawalApplication.IsApproved = true;

                    var usrBalance = repository.GetUserBalance(withdrawalApplication.UserId);
                    if (usrBalance.CurrentMoney < withdrawalApplication.Amount)
                        throw new Exception("Transfer couldn't be completed. Insufficient funds.");
                    usrBalance.CurrentMoney -= withdrawalApplication.Amount;

                    var withdrawalTransfer = Db.Set<OperationType>().Single(it => it.Code == "WITHDRAWAL");
                    var withdrawalOperation = new AccountOperation
                    {
                        UserId = withdrawalApplication.UserId,
                        OperationTypeId = withdrawalTransfer.Id,
                        OperationDate = DateTime.Now,
                        MoneyTransfered = withdrawalApplication.Amount
                    };
                    Db.Set<AccountOperation>().Add(withdrawalOperation);
                    Db.SaveChanges();
#if net452
                scope.Complete();
                }
                #endif
            }
            catch (Exception ex)
            {
                LogEventManager.Logger.Error(ex.Message, ex);
                throw;
            }
        }
    }
}
