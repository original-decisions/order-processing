using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using odec.Framework.Logging;
using odec.Processing.DAL;
using odec.Server.Model.OrderProcessing;
using odec.Server.Model.OrderProcessing.Contexts;
using odec.Server.Model.OrderProcessing.Filters;
using odec.Server.Model.OrderProcessing.Money;
using odec.Server.Model.User;

namespace Processing.Tests.Money
{
    public class SystemMoneyProcessingTester : Tester<EntireMoneyProcessingContext>
    {



        [Test]
        public void MakeTransferOrder()
        {
            try
            {
                var options = CreateNewContextOptions();
                using (var db = new EntireMoneyProcessingContext(options))
                {
                    ProcessingTestHelper.PopulateDefaultMoneyCtx(db);
                }
                using (var db = new EntireMoneyProcessingContext(options))
                {
                    var repository = new SystemMoneyProcessing(db);
                    // IocHelper.GetObject<ISystemMoneyProcessing>(db);
                    var paymentRepository = new PaymentMethodRepository(db);
                    //IocHelper.GetObject<IPaymentMethodRepository<int, DbContext, PaymentMethod, PaymentMethodFilter>>(db);


                    var payment =
                        paymentRepository.Get(new PaymentMethodFilter()).ToList()
                            .Single(it => it.Code == "CREDITPAYMENT");

                    var userId = db.Set<User>().First().Id;
                    Order result = null;
                    Assert.DoesNotThrow(() => result = repository.MakeTransferOrder(userId, 500, new OperationType(), payment));
                    Assert.NotNull(result);

                }
            }
            catch (Exception ex)
            {
                LogEventManager.Logger.Error(ex);
                throw;
            }
        }

        [Test]
        public void GetBalance()
        {
            try
            {
                var options = CreateNewContextOptions();
                using (var db = new EntireMoneyProcessingContext(options))
                {
                    ProcessingTestHelper.PopulateDefaultMoneyCtx(db);
                }

                using (var db = new EntireMoneyProcessingContext(options))
                {
                    var repository = new SystemMoneyProcessing(db);
                    //IocHelper.GetObject<ISystemMoneyProcessing>(db);


                    var user = db.Set<User>().First();
                    UserBalance result = null;
                    Assert.DoesNotThrow(() => result = repository.GetUserBalance(user));
                    Assert.NotNull(result);

                }
            }
            catch (Exception ex)
            {
                LogEventManager.Logger.Error(ex);
                throw;
            }
        }
        [Test]
        public void GetBalanceById()
        {
            try
            {
                var options = CreateNewContextOptions();
                using (var db = new EntireMoneyProcessingContext(options))
                {
                    ProcessingTestHelper.PopulateDefaultMoneyCtx(db);
                }

                using (var db = new EntireMoneyProcessingContext(options))
                {
                    var repository = new SystemMoneyProcessing(db);
                    //IocHelper.GetObject<ISystemMoneyProcessing>(db);


                    var user = db.Set<User>().First();
                    UserBalance result = null;
                    Assert.DoesNotThrow(() => result = repository.GetUserBalance(user.Id));
                    Assert.NotNull(result);

                }
            }
            catch (Exception ex)
            {
                LogEventManager.Logger.Error(ex);
                throw;
            }
        }

        [Test]
        public void GetTransferHistory()
        {
            try
            {
                var options = CreateNewContextOptions();
                using (var db = new EntireMoneyProcessingContext(options))
                {
                    ProcessingTestHelper.PopulateDefaultMoneyCtx(db);
                }

                using (var db = new EntireMoneyProcessingContext(options))
                {
                    var repository = new SystemMoneyProcessing(db);
                    //IocHelper.GetObject<ISystemMoneyProcessing>(db);


                    var user = db.Set<User>().First();
                    IEnumerable<AccountOperation> result = null;
                    Assert.DoesNotThrow(() => repository.AddFunds(user.Id, 1000));
                    Assert.DoesNotThrow(() => repository.TransferMoney(user.Id, 1000, user.Id));
                    Assert.DoesNotThrow(() => result = repository.GetTransferHistory(user.Id));
                    Assert.True(result != null && result.Count() > 0);

                }

            }
            catch (Exception ex)
            {
                LogEventManager.Logger.Error(ex);
                throw;
            }
        }
        [Test]
        public void BlockFunds()
        {
            try
            {
                var options = CreateNewContextOptions();
                using (var db = new EntireMoneyProcessingContext(options))
                {
                    ProcessingTestHelper.PopulateDefaultMoneyCtx(db);
                }

                using (var db = new EntireMoneyProcessingContext(options))
                {
                    var repository = new SystemMoneyProcessing(db);
                    //IocHelper.GetObject<ISystemMoneyProcessing>(db);


                    var user = db.Set<User>().First();

                    Assert.DoesNotThrow(() => repository.AddFunds(user.Id, 1000));
                    Assert.DoesNotThrow(() => repository.BlockFunds(user.Id, 1000));
                    Assert.True(repository.GetUserBalance(user).BlockedMoney == 1000);

                }

            }
            catch (Exception ex)
            {
                LogEventManager.Logger.Error(ex);
                throw;
            }
        }

        [Test]
        public void ReleaseBlockedMoney()
        {
            try
            {
                var options = CreateNewContextOptions();
                using (var db = new EntireMoneyProcessingContext(options))
                {
                    ProcessingTestHelper.PopulateDefaultMoneyCtx(db);
                }

                using (var db = new EntireMoneyProcessingContext(options))
                {
                    var repository = new SystemMoneyProcessing(db);
                    //IocHelper.GetObject<ISystemMoneyProcessing>(db);


                    var user = db.Set<User>().First();
                    Assert.DoesNotThrow(() => repository.AddFunds(user.Id, 1000));
                    Assert.DoesNotThrow(() => repository.BlockFunds(user.Id, 1000));
                    Assert.True(repository.GetUserBalance(user).BlockedMoney == 1000);
                    Assert.DoesNotThrow(() => repository.ReleaseBlockedMoney(user.Id, 1000));
                    Assert.True(repository.GetUserBalance(user).BlockedMoney == 0);

                }
            }
            catch (Exception ex)
            {
                LogEventManager.Logger.Error(ex);
                throw;
            }
        }

        [Test]
        public void TransferMoney()
        {
            try
            {
                var options = CreateNewContextOptions();
                using (var db = new EntireMoneyProcessingContext(options))
                {
                    ProcessingTestHelper.PopulateDefaultMoneyCtx(db);
                }

                using (var db = new EntireMoneyProcessingContext(options))
                {
                    var repository = new SystemMoneyProcessing(db);
                    //IocHelper.GetObject<ISystemMoneyProcessing>(db);


                    var user = db.Set<User>().First();

                    Assert.DoesNotThrow(() => repository.AddFunds(user.Id, 1000));
                    Assert.DoesNotThrow(() => repository.TransferMoney(user.Id, 1000, user.Id));

                }
            }
            catch (Exception ex)
            {
                LogEventManager.Logger.Error(ex);
                throw;
            }

        }

        [Test]
        public void TransferBlockedMoney()
        {
            try
            {
                var options = CreateNewContextOptions();
                User user;
                using (var db = new EntireMoneyProcessingContext(options))
                {
                    ProcessingTestHelper.PopulateDefaultMoneyCtx(db);
                    
                }
                using (var db = new EntireMoneyProcessingContext(options))
                {
                    user = db.Set<User>().First(it => it.UserName == "Andrew");
                    var repository = new SystemMoneyProcessing(db);
                    Assert.DoesNotThrow(() => repository.AddFunds(user.Id, 1000));
                    Assert.DoesNotThrow(() => repository.BlockFunds(user.Id, 1000));
                }
                using (var db = new EntireMoneyProcessingContext(options))
                {
                    var repository = new SystemMoneyProcessing(db);
                    Assert.DoesNotThrow(() => repository.TransferBlockedMoney(user.Id, 1000, user.Id));

                }
                using (var db = new EntireMoneyProcessingContext(options))
                {
                    user = db.Set<User>().First(it => it.UserName == "TestUser2");
                }
                using (var db = new EntireMoneyProcessingContext(options))
                {
                    var repository = new SystemMoneyProcessing(db);
                    Assert.DoesNotThrow(() => repository.AddFunds(user.Id, 1000));
                    Assert.DoesNotThrow(() => repository.BlockFunds(user.Id, 1000));
                }
                using (var db = new EntireMoneyProcessingContext(options))
                {
                    var repository = new SystemMoneyProcessing(db);
                    Assert.DoesNotThrow(() => repository.TransferBlockedMoney(user.Id, 1000, user.Id));

                }
            }
            catch (Exception ex)
            {
                LogEventManager.Logger.Error(ex);
                throw;
            }
        }

        [Test]
        public void AddFunds()
        {
            try
            {
                var options = CreateNewContextOptions();
                using (var db = new EntireMoneyProcessingContext(options))
                {
                    ProcessingTestHelper.PopulateDefaultMoneyCtx(db);
                }

                using (var db = new EntireMoneyProcessingContext(options))
                {
                    var repository = new SystemMoneyProcessing(db);
                    //IocHelper.GetObject<ISystemMoneyProcessing>(db);


                    var user = db.Set<User>().First();
                    var money = repository.GetUserBalance(user.Id).CurrentMoney;
                    Assert.DoesNotThrow(() => repository.AddFunds(user.Id, 1000));
                    Assert.True(repository.GetUserBalance(user.Id).CurrentMoney == money + 1000);

                }
            }
            catch (Exception ex)
            {
                LogEventManager.Logger.Error(ex);
                throw;
            }
        }
        [Test]
        public void GetReservedMoneyById()
        {
            try
            {
                var options = CreateNewContextOptions();
                using (var db = new EntireMoneyProcessingContext(options))
                {
                    ProcessingTestHelper.PopulateDefaultMoneyCtx(db);
                }

                using (var db = new EntireMoneyProcessingContext(options))
                {
                    var repository = new SystemMoneyProcessing(db);
                    //IocHelper.GetObject<ISystemMoneyProcessing>(db);


                    var user = db.Set<User>().First();
                    decimal money = -1;
                    Assert.DoesNotThrow(() => money = repository.GetReservedMoney(user.Id));

                }
            }
            catch (Exception ex)
            {
                LogEventManager.Logger.Error(ex);
                throw;
            }
        }
        [Test]
        public void GetReservedMoney()
        {
            try
            {
                var options = CreateNewContextOptions();
                using (var db = new EntireMoneyProcessingContext(options))
                {
                    ProcessingTestHelper.PopulateDefaultMoneyCtx(db);
                }

                using (var db = new EntireMoneyProcessingContext(options))
                {
                    var repository = new SystemMoneyProcessing(db);
                    //IocHelper.GetObject<ISystemMoneyProcessing>(db);


                    var user = db.Set<User>().First();
                    decimal money = -1;
                    Assert.DoesNotThrow(() => money = repository.GetReservedMoney(user));

                }
            }
            catch (Exception ex)
            {
                LogEventManager.Logger.Error(ex);
                throw;
            }
        }
    }
}
