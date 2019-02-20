using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Microsoft.EntityFrameworkCore;
#if net452
using System.Transactions;
#endif
using Newtonsoft.Json;
using odec.Entity.DAL.Interop;
using odec.Framework.Logging;
using odec.Processing.DAL.Interop;
using odec.Server.Model.Contact;
using odec.Server.Model.OrderProcessing;
using odec.Server.Model.OrderProcessing.Money;
using odec.Server.Model.User;

namespace odec.Processing.DAL
{
    public class SystemMoneyProcessing : ISystemMoneyProcessing<int, DbContext, User, UserBalance, OperationType, Order, AccountOperation, PaymentMethod>, IContextRepository<DbContext>
    {
        private const string eMoneyOrderPrefix = "eMoney";
        public SystemMoneyProcessing()
        {

        }

        public SystemMoneyProcessing(DbContext db)
        {
            Db = db;
        }
        public DbContext Db { get; set; }
        public void SetConnection(string connection)
        {
            throw new NotImplementedException();
        }

        public void SetContext(DbContext db)
        {
            Db = db;
        }

        public UserBalance GetUserBalance(int userId)
        {
            try
            {
                return (from usrAcc in Db.Set<UserMAccount>()
                        join user in Db.Set<User>() on usrAcc.UserId equals user.Id
                        where usrAcc.UserId == userId
                        select new UserBalance
                        {
                            BlockedMoney = usrAcc.BlockedMoney,
                            CurrentMoney = usrAcc.CurrentMoney,
                            UserId = usrAcc.UserId,
                            User = user
                        }).Single();
            }
            catch (Exception ex)
            {
                LogEventManager.Logger.Error(ex.Message, ex);
                throw;
            }
        }

        public UserBalance GetUserBalance(User user)
        {
            try
            {
                return GetUserBalance(user.Id);

            }
            catch (Exception ex)
            {
                LogEventManager.Logger.Error(ex.Message, ex);
                throw;
            }
        }

        private UserMAccount MakeUserMoneyAccountIfNeeded(int userId, decimal amount, bool isIntantSave)
        {
            var userMoney = Db.Set<UserMAccount>().SingleOrDefault(it => it.UserId == userId);
            if (userMoney != null) return userMoney;
            userMoney = new UserMAccount
            {
                UserId = userId,
                BlockedMoney = 0,
                CurrentMoney = amount
            };
            Db.Set<UserMAccount>().Add(userMoney);
            if (isIntantSave)
            {
                Db.SaveChanges();
            }
            return userMoney;
        }
        public Order MakeTransferOrder(int userId, decimal amount, OperationType transferType, PaymentMethod payment)
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
                var orderNumber = eMoneyOrderPrefix + "_" + userId + "_" + amount.ToString("F") +
                              "_" + DateTime.Now.Ticks;
                var orderState = Db.Set<OrderState>().Single(it => it.Code == "FORMED");
                var deliveryMethod = Db.Set<DeliveryMethod>().Single(it => it.Code == "ELECTRONIC");
                var paymentMethod = payment.Id != 0
                    ? Db.Set<PaymentMethod>().Single(it => it.Id == payment.Id)
                    : Db.Set<PaymentMethod>().Single(it => it.Code == payment.Code);

                //
                var contact = (from uContact in Db.Set<UserContact>()
                                   //   join contact1 in Db.Set<Contact>() on uContact.ContactId equals contact1.Id
                               select uContact).FirstOrDefault();


                if (contact == null)
                {
                    var user = Db.Set<User>().Single(it => it.Id == userId);
                    var noSex = Db.Set<Sex>().SingleOrDefault(it => it.Code == "NOSEX");
                    if (noSex == null)
                    {
                        noSex = new Sex
                        {
                            Code = "NOSEX",
                            DateCreated = DateTime.Now,
                            IsActive = true,
                            Name = "No Sex selected",
                            SortOrder = 0
                        };
                        Db.Set<Sex>().Add(noSex);
                    }
                    contact = new UserContact
                    {
                        UserId = userId,
                        IsAccountBased = true,
                        Contact = new Contact
                        {
                            Code = userId + "_" + Guid.NewGuid(),
                            Email = user.Email,
                            FirstName = user.FirstName ?? string.Empty,
                            LastName = user.LastName ?? string.Empty,
                            Name = "Contact Number: " + userId + "_" + Guid.NewGuid(),
                            DateCreated = DateTime.Now,
                            IsActive = true,
                            Patronymic = user.Patronymic ?? string.Empty,
                            SexId = noSex.Id,
                            SortOrder = 0,
                            SendNews = false,
                            AddressDenormolized = string.Empty,
                            PhoneNumberDenormolized = user.PhoneNumber ?? string.Empty
                        }
                    };
                    Db.Set<UserContact>().Add(contact);
                }
                var order = new Order
                {
                    Code = orderNumber,
                    Comment = String.Empty,
                    Name = orderNumber,
                    DateCreated = DateTime.Now,
                    IsActive = true,
                    OrderStateId = orderState.Id,
                    //todo:delete order date. it is the same as DateCreated.
                    OrderDate = DateTime.Now,
                    Total = amount,
                    SortOrder = 0,
                    DateDelivery = DateTime.Today,
                    DeliveryMethodId = deliveryMethod.Id,
                    PaymentMethodId = paymentMethod.Id,
                    ContactId = contact.ContactId
                };
                Db.Set<Order>().Add(order);

                var operationType = Db.Set<OperationType>().Single(it => it.Code == "CASHDEPOSIT");
                var aOperation = new AccountOperation
                {
                    UserId = userId,
                    OperationTypeId = operationType.Id,
                    OperationDate = DateTime.Now,
                    MoneyTransfered = amount
                };
                Db.Set<AccountOperation>().Add(aOperation);
                var userMoney = MakeUserMoneyAccountIfNeeded(userId, amount, false);
                var orderDetail = new OrderDetail
                {
                    DiscountedCost = amount,
                    EntityCount = 1,
                    MomentCost = amount,
                    JsonEntityDetails = JsonConvert.SerializeObject(userMoney),
                    OrderId = order.Id,
                    Total = amount
                };
                var orderType = Db.Set<OrderType>().Single(it => it.Code.Equals("MONEYORDER"));
                Db.Set<OrderOrderType>().Add(new OrderOrderType
                {
                    OrderId = order.Id,
                    OrderTypeId = orderType.Id
                });
                Db.Set<OrderDetail>().Add(orderDetail);
                Db.SaveChanges();
#if net452
                scope.Complete();
#endif
                return order;
#if net452
                }
#endif
            }
            catch (Exception ex)
            {
                LogEventManager.Logger.Error(ex.Message, ex);
                throw;
            }
        }

        public IEnumerable<AccountOperation> GetTransferHistory(int userId, OperationType transferType = null)
        {
            try
            {
                return from accountOperation in Db.Set<AccountOperation>()
                       join operationType in Db.Set<OperationType>() on accountOperation.OperationTypeId equals
                           operationType.Id
                       join user in Db.Set<User>() on accountOperation.UserId equals user.Id

                       where transferType == null || accountOperation.OperationTypeId == transferType.Id
                       select new AccountOperation
                       {
                           User = user,
                           OperationType = operationType,
                           MoneyTransfered = accountOperation.MoneyTransfered,
                           OperationDate = accountOperation.OperationDate,
                           UserId = accountOperation.UserId,
                           OperationTypeId = accountOperation.OperationTypeId
                       };

            }
            catch (Exception ex)
            {
                LogEventManager.Logger.Error(ex.Message, ex);
                throw;
            }
        }

        public UserBalance BlockFunds(int userId, decimal amount)
        {
            try
            {
                var usrBalance = Db.Set<UserMAccount>().Include(it => it.User).Single(it => it.UserId == userId); //GetUserBalance(userId);

                if (usrBalance.CurrentMoney < amount)
                    throw new Exception("Insufficient funds");
                usrBalance.CurrentMoney -= amount;
                usrBalance.BlockedMoney += amount;
                Db.SaveChanges();
                return new UserBalance
                {
                    BlockedMoney = usrBalance.BlockedMoney,
                    CurrentMoney = usrBalance.CurrentMoney,
                    UserId = usrBalance.UserId,
                    User = usrBalance.User
                }; ;
            }
            catch (Exception ex)
            {
                LogEventManager.Logger.Error(ex.Message, ex);
                throw;
            }
        }

        public UserBalance ReleaseBlockedMoney(int userId, decimal amount)
        {
            try
            {
                var usrBalance = Db.Set<UserMAccount>().Include(it => it.User).Single(it => it.UserId == userId); //GetUserBalance(userId);

                if (usrBalance.BlockedMoney < amount)
                    throw new Exception("Operation couldn't be completed");
                usrBalance.CurrentMoney += amount;
                usrBalance.BlockedMoney -= amount;
                Db.SaveChanges();
                return new UserBalance
                {
                    BlockedMoney = usrBalance.BlockedMoney,
                    CurrentMoney = usrBalance.CurrentMoney,
                    UserId = usrBalance.UserId,
                    User = usrBalance.User
                }; ;
            }
            catch (Exception ex)
            {
                LogEventManager.Logger.Error(ex.Message, ex);
                throw;
            }
        }

        public Tuple<UserBalance, UserBalance> TransferMoney(int userId, decimal amount, int destinationUserId)
        {
            try
            {
                var usrBalance = GetUserBalance(userId);
                if (usrBalance.CurrentMoney < amount)
                    throw new Exception("Transfer couldn't be completed. Insufficient funds.");
                usrBalance.CurrentMoney -= amount;

                var userToUserTransfer = Db.Set<OperationType>().Single(it => it.Code == "USERTOUSERTRANSFER");
                var u2uOperation = new AccountOperation
                {
                    UserId = userId,
                    OperationTypeId = userToUserTransfer.Id,
                    OperationDate = DateTime.Now,
                    MoneyTransfered = amount
                };
                Db.Set<AccountOperation>().Add(u2uOperation);
                var refillTransfer = Db.Set<OperationType>().Single(it => it.Code == "REFILL");
                var refillOperation = new AccountOperation
                {
                    UserId = destinationUserId,
                    OperationTypeId = refillTransfer.Id,
                    OperationDate = DateTime.Now,
                    MoneyTransfered = amount
                };
                Db.Set<AccountOperation>().Add(refillOperation);
                var userMoney = MakeUserMoneyAccountIfNeeded(destinationUserId, amount, false);
                userMoney.CurrentMoney += amount;
                Db.SaveChanges();
                var destinationUserBalance = new UserBalance
                {
                    UserId = userId,
                    BlockedMoney = userMoney.BlockedMoney,
                    CurrentMoney = userMoney.CurrentMoney
                };
                return new Tuple<UserBalance, UserBalance>(usrBalance, destinationUserBalance);
            }
            catch (Exception ex)
            {
                LogEventManager.Logger.Error(ex.Message, ex);
                throw;
            }
        }

        public Tuple<UserBalance, UserBalance> TransferBlockedMoney(int userId, decimal amount, int destinationUserId)
        {
            try
            {
                var usrBalance = GetUserBalance(userId);
                if (usrBalance.BlockedMoney < amount)
                    throw new Exception("Transfer couldn't be completed. Insufficient funds.");
                usrBalance.BlockedMoney -= amount;

                var userToUserTransfer = Db.Set<OperationType>().Single(it => it.Code == "USERTOUSERTRANSFER");
                var u2uOperation = new AccountOperation
                {
                    UserId = userId,
                    OperationTypeId = userToUserTransfer.Id,
                    OperationDate = DateTime.Now,
                    MoneyTransfered = amount
                };
                Db.Set<AccountOperation>().Add(u2uOperation);
                var refillTransfer = Db.Set<OperationType>().Single(it => it.Code == "REFILL");
                var refillOperation = new AccountOperation
                {
                    UserId = destinationUserId,
                    OperationTypeId = refillTransfer.Id,
                    OperationDate = DateTime.Now.AddSeconds(2),
                    MoneyTransfered = amount
                };
                Db.Set<AccountOperation>().Add(refillOperation);
                var userMoney = MakeUserMoneyAccountIfNeeded(destinationUserId, amount, false);
                userMoney.CurrentMoney += amount;
                Db.SaveChanges();
                var destinationUserBalance = new UserBalance
                {
                    UserId = userId,
                    BlockedMoney = userMoney.BlockedMoney,
                    CurrentMoney = userMoney.CurrentMoney
                };
                return new Tuple<UserBalance, UserBalance>(usrBalance, destinationUserBalance);
            }
            catch (Exception ex)
            {
                LogEventManager.Logger.Error(ex.Message, ex);
                throw;
            }
        }

        public UserBalance AddFunds(int userId, decimal amount)
        {
            try
            {
                try
                {
                    var usrBalance = Db.Set<UserMAccount>().Include(it=>it.User).Single(it => it.UserId == userId); //GetUserBalance(userId);
                    usrBalance.CurrentMoney += amount;
                    Db.SaveChanges();
                    return new UserBalance
                    {
                        BlockedMoney = usrBalance.BlockedMoney,
                        CurrentMoney = usrBalance.CurrentMoney,
                        UserId = usrBalance.UserId,
                        User = usrBalance.User
                    };
                }
                catch (Exception ex)
                {
                    LogEventManager.Logger.Error(ex.Message, ex);
                    throw;
                }
            }
            catch (Exception ex)
            {
                LogEventManager.Logger.Error(ex.Message, ex);
                throw;
            }
        }

        public decimal GetReservedMoney(int userId)
        {
            try
            {
                return GetUserBalance(userId).BlockedMoney;

            }
            catch (Exception ex)
            {
                LogEventManager.Logger.Error(ex.Message, ex);
                throw;
            }
        }

        public decimal GetReservedMoney(User user)
        {
            try
            {
                return GetReservedMoney(user.Id);

            }
            catch (Exception ex)
            {
                LogEventManager.Logger.Error(ex.Message, ex);
                throw;
            }
        }
    }
}
