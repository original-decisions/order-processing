using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using NUnit.Framework;
using odec.Framework.Generic.Utility;
using odec.Framework.Logging;
using odec.Processing.DAL;
using odec.Server.Model.OrderProcessing;
using odec.Server.Model.OrderProcessing.Contexts;
using odec.Server.Model.OrderProcessing.Filters;
using odec.Server.Model.OrderProcessing.Money;
using odec.Server.Model.OrderProcessing.Money.Withdrawal;
using odec.Server.Model.User;
#if net452
using System.Transactions;
#endif

namespace Processing.Tests.Money
{
    public class WithdrawalProcessingTester : Tester<EntireMoneyProcessingContext>
    {


        [Test]
        public void GetById()
        {
            try
            {
                var options = CreateNewContextOptions();
                using (var db = new EntireMoneyProcessingContext(options))
                {
                    ProcessingTestHelper.PopulateDefaultMoneyCtx(db);

                }
                WithdrawalApplication result = null;
                using (var db = new EntireMoneyProcessingContext(options))
                {
                    var repository = new WithdrawalProcessing(db);
                    Assert.DoesNotThrow(() => result = repository.GetById(db.WithdrawalApplications.First().Id));
                    Assert.True(result != null && result.Id > 0);
                }
            }
            catch (Exception ex)
            {
                LogEventManager.Logger.Error(ex.Message, ex);
                throw;
            }
        }
        [Test]
        [TestCase("COURIER", "Andrew", ExpectedResult = true)]
        [TestCase("CREDITCARD", "Andrew", ExpectedResult = true)]
        public bool CreateApplication(string withdrawalMethodCode, string userName)
        {
            try
            {
                var options = CreateNewContextOptions();
                User user;
                WithdrawalMethod withdrawalMethod;
                using (var db = new EntireMoneyProcessingContext(options))
                {
                    ProcessingTestHelper.PopulateDefaultMoneyCtx(db);

                }
                WithdrawalApplication result = null;
                using (var db = new EntireMoneyProcessingContext(options))
                {
                    var repository = new WithdrawalProcessing(db);
                    user = db.Set<User>().First(it => it.UserName == userName);
                    withdrawalMethod = db.Set<WithdrawalMethod>().First(it => it.Code == withdrawalMethodCode);
                    Assert.DoesNotThrow(() => result = repository.CreateApplication(ProcessingTestHelper.GenerateWithdrawalApplication(user, withdrawalMethod)));
                    return result != null && result.Id > 0;
                }
            }
            catch (Exception ex)
            {
                LogEventManager.Logger.Error(ex.Message, ex);
                throw;
            }
        }


        public class CreateApplicationTestCasesObject : IEnumerable
        {
            public IEnumerator GetEnumerator()
            {
                var options = CreateNewContextOptions();
                WithdrawalMethod courierMethod;
                WithdrawalMethod creditCardMethod;
                User user;
                using (var db = new EntireMoneyProcessingContext(options))
                {
                    ProcessingTestHelper.PopulateDefaultMoneyCtx(db);
                    courierMethod = db.Set<WithdrawalMethod>().First(it => it.Code == "courier".ToUpper());
                    creditCardMethod = db.Set<WithdrawalMethod>().First(it => it.Code == "creditCard".ToUpper());
                    user = db.Set<User>().First(it => it.UserName == "Andrew");
                }
                var obj1 = new TestComplexObject
                {
                    WithdrawalMethod = courierMethod,
                    User = user,
                    WithdrawalBefore = DateTime.Now.AddDays(15),
                    Amount = 100M,
                    Comment = "MyComment"
                };
                var obj2 = new TestComplexObject
                {
                    WithdrawalMethod = creditCardMethod,
                    User = user,
                    WithdrawalBefore = DateTime.Now.AddDays(15),
                    Amount = 100M,
                    Comment = "MyComment"
                };
                yield return
                    new TestCaseData(options, obj1).SetName("CreateApplication: " +
                                                            JsonConvert.SerializeObject(obj1).Replace('.', ','));
                yield return
                    new TestCaseData(options, obj2).SetName("CreateApplication: " +
                                                            JsonConvert.SerializeObject(obj2).Replace('.', ','));
            }
        }

        public class TestComplexObject
        {
            public WithdrawalMethod WithdrawalMethod { get; set; }
            public User User { get; set; }
            public DateTime WithdrawalBefore { get; set; }
            public decimal Amount { get; set; }
            public string Comment { get; set; }
        }
        public class CreateApplicationTestCasesIds : IEnumerable
        {
            public IEnumerator GetEnumerator()
            {
                var options = CreateNewContextOptions();
                WithdrawalMethod courierMethod;
                WithdrawalMethod creditCardMethod;
                User user;
                using (var db = new EntireMoneyProcessingContext(options))
                {
                    ProcessingTestHelper.PopulateDefaultMoneyCtx(db);
                    courierMethod = db.Set<WithdrawalMethod>().First(it => it.Code == "courier".ToUpper());
                    creditCardMethod = db.Set<WithdrawalMethod>().First(it => it.Code == "creditCard".ToUpper());
                    user = db.Set<User>().First(it => it.UserName == "Andrew");
                }
                var serializerSettings = new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore
                    // DateFormatHandling = DateFormatHandling.MicrosoftDateFormat
                };

                yield return new TestCaseData(options, new CreateApplicationIdTestObj
                {
                    WithdrawalMethodId = courierMethod.Id,
                    UserId = user.Id,
                    WithdrawalBefore = DateTime.Now.AddDays(15),
                    Amount = 100M,
                    Comment = null
                })
                    .SetName("CreateAplicationById: Case 1" + JsonConvert.SerializeObject(new CreateApplicationIdTestObj
                    {
                        WithdrawalMethodId = courierMethod.Id,
                        UserId = user.Id,
                        WithdrawalBefore = DateTime.Now.AddDays(15),
                        Amount = 100M,
                        Comment = null
                    }, serializerSettings).Replace('.', ','));
                yield return new TestCaseData(options, new CreateApplicationIdTestObj
                {
                    WithdrawalMethodId = creditCardMethod.Id,
                    UserId = user.Id,
                    WithdrawalBefore = DateTime.Now.AddDays(15),
                    Amount = 100M,
                    Comment = string.Empty
                })
                    .SetName("CreateAplication: " + JsonConvert.SerializeObject(new CreateApplicationIdTestObj
                    {
                        WithdrawalMethodId = creditCardMethod.Id,
                        UserId = user.Id,
                        WithdrawalBefore = DateTime.Now.AddDays(15),
                        Amount = 100M,
                        Comment = string.Empty
                    }, serializerSettings).Replace('.', ',')); ;
                yield return new TestCaseData(options, new CreateApplicationIdTestObj
                {
                    WithdrawalMethodId = courierMethod.Id,
                    UserId = user.Id,
                    WithdrawalBefore = DateTime.Now.AddDays(15),
                    Amount = 100M,
                    Comment = "MyComment"
                }).SetName("CreateAplication: " + JsonConvert.SerializeObject(new CreateApplicationIdTestObj
                {
                    WithdrawalMethodId = courierMethod.Id,
                    UserId = user.Id,
                    WithdrawalBefore = DateTime.Now.AddDays(15),
                    Amount = 100M,
                    Comment = "MyComment"
                }, serializerSettings).Replace('.', ','));

            }
        }
        public class CreateApplicationIdTestObj
        {
            public int WithdrawalMethodId { get; set; }
            public int UserId { get; set; }
            public DateTime WithdrawalBefore { get; set; }
            public decimal Amount { get; set; }
            public string Comment { get; set; }
        }
        [Test]
        [TestCaseSource(typeof(CreateApplicationTestCasesObject))]
        public void CreateApplication2(DbContextOptions<EntireMoneyProcessingContext> options, TestComplexObject obj)
        {
            try
            {

                WithdrawalApplication result = null;
                using (var db = new EntireMoneyProcessingContext(options))
                {
                    var repository = new WithdrawalProcessing(db);
                    Assert.DoesNotThrow(
                        () =>
                            result =
                                repository.CreateApplication(obj.WithdrawalMethod, obj.User, obj.WithdrawalBefore, obj.Amount, obj.Comment));
                    Assert.True(result != null && result.Id > 0);
                }
            }
            catch (Exception ex)
            {
                LogEventManager.Logger.Error(ex.Message, ex);
                throw;
            }
        }
        [Test]
        [TestCaseSource(typeof(CreateApplicationTestCasesIds))]
        public void CreateApplicationByWithdrawalMethodAndUserIds(DbContextOptions<EntireMoneyProcessingContext> options, CreateApplicationIdTestObj obj)
        {
            try
            {

                WithdrawalApplication result = null;
                using (var db = new EntireMoneyProcessingContext(options))
                {
                    var repository = new WithdrawalProcessing(db);
                    Assert.DoesNotThrow(
                        () =>
                            result =
                                repository.CreateApplication(obj.WithdrawalMethodId, obj.UserId, obj.WithdrawalBefore, obj.Amount,
                                    obj.Comment));
                    Assert.True(result != null && result.Id > 0);
                }
            }
            catch (Exception ex)
            {
                LogEventManager.Logger.Error(ex.Message, ex);
                throw;
            }
        }

        public class GetApplicationCases : IEnumerable
        {
            public IEnumerator GetEnumerator()
            {
                var options = CreateNewContextOptions();
                WithdrawalMethod courierMethod;
                WithdrawalMethod creditCardMethod;
                User user;
                using (var db = new EntireMoneyProcessingContext(options))
                {
                    ProcessingTestHelper.PopulateDefaultMoneyCtx(db);
                    courierMethod = db.Set<WithdrawalMethod>().First(it => it.Code == "courier".ToUpper());
                    creditCardMethod = db.Set<WithdrawalMethod>().First(it => it.Code == "creditCard".ToUpper());
                    user = db.Set<User>().First(it => it.UserName == "Andrew");
                }
                var serializerSettings = new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore,
                    DateFormatString = "dd-MM-yyyy"//,
                                                   //   Culture = new CultureInfo("ru-ru")

                    //DateFormatHandling = DateFormatHandling.MicrosoftDateFormat

                };
                var withdrawalFilter = new WithdrawalApplicationFilter<int, int?>
                {
                    UserId = user.Id
                };
                yield return new TestCaseData(options, withdrawalFilter, 5)
                    .SetName("GetApplications: Filter -> " + JsonConvert.SerializeObject(withdrawalFilter, serializerSettings));

                var usercourierMethodFilter = new WithdrawalApplicationFilter<int, int?>
                {
                    UserId = user.Id,
                    WithdrawalMethodId = courierMethod.Id
                };
                yield return new TestCaseData(options, usercourierMethodFilter, 3)
                    .SetName("GetApplications: Filter -> " + JsonConvert.SerializeObject(usercourierMethodFilter, serializerSettings));
                var userCreditCardFilter = new WithdrawalApplicationFilter<int, int?>
                {
                    UserId = user.Id,
                    WithdrawalMethodId = creditCardMethod.Id
                };
                yield return new TestCaseData(options, userCreditCardFilter, 2)
                    .SetName("GetApplications: Filter -> " + JsonConvert.SerializeObject(userCreditCardFilter, serializerSettings));

                var userCorierDateIntervalFilter1 = new WithdrawalApplicationFilter<int, int?>
                {
                    UserId = user.Id,
                    WithdrawalMethodId = courierMethod.Id,
                    DateCreatedInterval = new Interval<DateTime?>() { End = DateTime.Now.AddDays(200) }
                };
                yield return new TestCaseData(options, userCorierDateIntervalFilter1, 3)
                    .SetName("GetApplications: Filter -> " + JsonConvert.SerializeObject(userCorierDateIntervalFilter1, serializerSettings));
                var userCorierDateIntervalFilter2 = new WithdrawalApplicationFilter<int, int?>
                {
                    UserId = user.Id,
                    WithdrawalMethodId = courierMethod.Id,
                    DateCreatedInterval = new Interval<DateTime?>() { Start = DateTime.Now.AddDays(-14) }
                };

                yield return new TestCaseData(options, userCorierDateIntervalFilter2, 3)
                    .SetName("GetApplications: Filter -> " + JsonConvert.SerializeObject(userCorierDateIntervalFilter2, serializerSettings));

                var userCorierDateIntervalFilter3 = new WithdrawalApplicationFilter<int, int?>
                {
                    UserId = user.Id,
                    WithdrawalMethodId = courierMethod.Id,
                    DateCreatedInterval =
                        new Interval<DateTime?>() { End = DateTime.Now.AddDays(7), Start = DateTime.Now.AddDays(-14) }
                };
                yield return new TestCaseData(options, userCorierDateIntervalFilter3, 3)
                    .SetName("GetApplications: Filter -> " + JsonConvert.SerializeObject(userCorierDateIntervalFilter3, serializerSettings));

                var userCorierDateIntervalWithdrawalDateFilter1 = new WithdrawalApplicationFilter<int, int?>
                {
                    UserId = user.Id,
                    WithdrawalMethodId = courierMethod.Id,
                    DateCreatedInterval =
                        new Interval<DateTime?> { End = DateTime.Now.AddDays(7), Start = DateTime.Now.AddDays(-14) },
                    WithdrawalBeforeInterval =
                        new Interval<DateTime?> { End = DateTime.Now.AddDays(15), Start = DateTime.Now.AddDays(-14) }
                };
                yield return new TestCaseData(options, userCorierDateIntervalWithdrawalDateFilter1, 3)
                    .SetName("GetApplications: Filter -> " + JsonConvert.SerializeObject(userCorierDateIntervalWithdrawalDateFilter1, serializerSettings));

                var userCorierDateIntervalWithdrawalDateAmountFilter1 = new WithdrawalApplicationFilter<int, int?>
                {
                    UserId = user.Id,
                    WithdrawalMethodId = courierMethod.Id,
                    DateCreatedInterval = new Interval<DateTime?> { End = DateTime.Now.AddDays(7), Start = DateTime.Now.AddDays(-14) },
                    WithdrawalBeforeInterval = new Interval<DateTime?> { End = DateTime.Now.AddDays(15), Start = DateTime.Now.AddDays(-14) },
                    Amount = new Interval<decimal?>() { Start = 0 }
                };
                yield return new TestCaseData(options, userCorierDateIntervalWithdrawalDateAmountFilter1, 3)
                    .SetName("GetApplications: Filter -> " + JsonConvert.SerializeObject(userCorierDateIntervalWithdrawalDateAmountFilter1, serializerSettings));

                var userCorierDateIntervalWithdrawalDateAmountFilter2 = new WithdrawalApplicationFilter<int, int?>
                {
                    UserId = user.Id,
                    WithdrawalMethodId = courierMethod.Id,
                    DateCreatedInterval =
                        new Interval<DateTime?> { End = DateTime.Now.AddDays(7), Start = DateTime.Now.AddDays(-14) },
                    WithdrawalBeforeInterval =
                        new Interval<DateTime?> { End = DateTime.Now.AddDays(7), Start = DateTime.Now.AddDays(-14) },
                    Amount = new Interval<decimal?>() { Start = 0 }
                };
                yield return new TestCaseData(options, userCorierDateIntervalWithdrawalDateAmountFilter2, 0)
                    .SetName("GetApplications: Filter -> " + JsonConvert.SerializeObject(userCorierDateIntervalWithdrawalDateAmountFilter2, serializerSettings));
                var userCorierDateIntervalWithdrawalDateAmountFilter3 = new WithdrawalApplicationFilter<int, int?>
                {
                    UserId = user.Id,
                    WithdrawalMethodId = courierMethod.Id,
                    DateCreatedInterval =
                        new Interval<DateTime?> { End = DateTime.Now.AddDays(7), Start = DateTime.Now.AddDays(-14) },
                    WithdrawalBeforeInterval =
                        new Interval<DateTime?> { End = DateTime.Now.AddDays(15), Start = DateTime.Now.AddDays(-14) },
                    Amount = new Interval<decimal?>() { Start = 0 }
                };
                yield return new TestCaseData(options, userCorierDateIntervalWithdrawalDateAmountFilter3, 3)
                    .SetName("GetApplications: Filter -> " + JsonConvert.SerializeObject(userCorierDateIntervalWithdrawalDateAmountFilter3, serializerSettings));

                var userCorierDateIntervalWithdrawalDateAmountFilter4 = new WithdrawalApplicationFilter<int, int?>
                {
                    UserId = user.Id,
                    WithdrawalMethodId = courierMethod.Id,
                    DateCreatedInterval =
                        new Interval<DateTime?> { End = DateTime.Now.AddDays(7), Start = DateTime.Now.AddDays(-14) },
                    WithdrawalBeforeInterval =
                        new Interval<DateTime?> { End = DateTime.Now.AddDays(15), Start = DateTime.Now.AddDays(-14) },
                    Amount = new Interval<decimal?>() { Start = 0, End = 100000 }
                };
                yield return new TestCaseData(options, userCorierDateIntervalWithdrawalDateAmountFilter4, 3)
                    .SetName("GetApplications: Filter -> " + JsonConvert.SerializeObject(userCorierDateIntervalWithdrawalDateAmountFilter4, serializerSettings));
                var userCorierDateIntervalWithdrawalDateAmountFilter5 = new WithdrawalApplicationFilter<int, int?>
                {
                    UserId = user.Id,
                    WithdrawalMethodId = courierMethod.Id,
                    DateCreatedInterval =
                        new Interval<DateTime?> { End = DateTime.Now.AddDays(7), Start = DateTime.Now.AddDays(-14) },
                    WithdrawalBeforeInterval =
                        new Interval<DateTime?> { End = DateTime.Now.AddDays(15), Start = DateTime.Now.AddDays(-14) },
                    Amount = new Interval<decimal?>() { Start = 1001, End = 100000 }
                };
                yield return new TestCaseData(options, userCorierDateIntervalWithdrawalDateAmountFilter5, 0)
                    .SetName("GetApplications: Filter -> " + JsonConvert.SerializeObject(userCorierDateIntervalWithdrawalDateAmountFilter5, serializerSettings));
                ;
                var userCorierDateIntervalWithdrawalDateAmountFilter6 = new WithdrawalApplicationFilter<int, int?>
                {
                    UserId = user.Id,
                    WithdrawalMethodId = courierMethod.Id,
                    DateCreatedInterval =
                        new Interval<DateTime?> { End = DateTime.Now.AddDays(7), Start = DateTime.Now.AddDays(-14) },
                    WithdrawalBeforeInterval =
                        new Interval<DateTime?> { End = DateTime.Now.AddDays(15), Start = DateTime.Now.AddDays(-14) },
                    Amount = new Interval<decimal?> { Start = 1001 }
                };
                yield return new TestCaseData(options, userCorierDateIntervalWithdrawalDateAmountFilter6, 0)
                    .SetName("GetApplications: Filter -> " + JsonConvert.SerializeObject(userCorierDateIntervalWithdrawalDateAmountFilter6, serializerSettings));
                var userCorierDateIntervalWithdrawalDateAmountFilter7 = new WithdrawalApplicationFilter<int, int?>
                {
                    UserId = user.Id,
                    WithdrawalMethodId = courierMethod.Id,
                    DateCreatedInterval =
                        new Interval<DateTime?> { End = DateTime.Now.AddDays(7), Start = DateTime.Now.AddDays(-14) },
                    WithdrawalBeforeInterval =
                        new Interval<DateTime?> { End = DateTime.Now.AddDays(15), Start = DateTime.Now.AddDays(-14) },
                    Amount = new Interval<decimal?>() { End = 1001 }
                };
                yield return new TestCaseData(options, userCorierDateIntervalWithdrawalDateAmountFilter7, 3)
                    .SetName("GetApplications: Filter -> " + JsonConvert.SerializeObject(userCorierDateIntervalWithdrawalDateAmountFilter7, serializerSettings));

                var userCorierDateIntervalWithdrawalDateAmountFilter8 = new WithdrawalApplicationFilter<int, int?>
                {
                    UserId = user.Id,
                    WithdrawalMethodId = courierMethod.Id,
                    DateCreatedInterval =
                        new Interval<DateTime?> { End = DateTime.Now.AddDays(7), Start = DateTime.Now.AddDays(-14) },
                    WithdrawalBeforeInterval =
                        new Interval<DateTime?> { End = DateTime.Now.AddDays(15), Start = DateTime.Now.AddDays(-14) },
                    Amount = new Interval<decimal?>() { Start = 99, End = 999 }
                };
                yield return new TestCaseData(options, userCorierDateIntervalWithdrawalDateAmountFilter8, 0)
                    .SetName("GetApplications: Filter -> " + JsonConvert.SerializeObject(userCorierDateIntervalWithdrawalDateAmountFilter8, serializerSettings));

                var userCorierDateIntervalWithdrawalDateAmountFilter9 = new WithdrawalApplicationFilter<int, int?>
                {
                    UserId = user.Id,
                    WithdrawalMethodId = courierMethod.Id,
                    DateCreatedInterval =
                        new Interval<DateTime?> { End = DateTime.Now.AddDays(7), Start = DateTime.Now.AddDays(-14) },
                    WithdrawalBeforeInterval =
                        new Interval<DateTime?> { End = DateTime.Now.AddDays(15), Start = DateTime.Now.AddDays(-14) },
                    Amount = new Interval<decimal?>() { Start = 99, End = 999 }
                };
                yield return new TestCaseData(options, userCorierDateIntervalWithdrawalDateAmountFilter9, 0)
                    .SetName("GetApplications: Filter -> " + JsonConvert.SerializeObject(userCorierDateIntervalWithdrawalDateAmountFilter9, serializerSettings));
                ;
            }
        }

        [Test]
        [TestCaseSource(typeof(GetApplicationCases))]
        public void GetApplications(DbContextOptions<EntireMoneyProcessingContext> options, WithdrawalApplicationFilter<int, int?> filter, int expectedCount)
        {
            try
            {
                IEnumerable<WithdrawalApplication> result = null;
                using (var db = new EntireMoneyProcessingContext(options))
                {
                    var repository = new WithdrawalProcessing(db);
                    Assert.DoesNotThrow(
                        () => result =
                                repository.GetApplications(filter));
                    Assert.True(result != null && result.Count() == expectedCount);
                }
            }
            catch (Exception ex)
            {
                LogEventManager.Logger.Error(ex.Message, ex);
                throw;
            }
        }
        [Test]
        public void ApproveWithdrawal()
        {
            try
            {
                var options = CreateNewContextOptions();
                WithdrawalApplication item;
                using (var db = new EntireMoneyProcessingContext(options))
                {
                    ProcessingTestHelper.PopulateDefaultMoneyCtx(db);
                    var acc = db.UsersAccount.First();
                    acc.CurrentMoney = 1000;
                    db.SaveChanges();
                }
                using (var db = new EntireMoneyProcessingContext(options))
                {
                    var repository = new WithdrawalProcessing(db);
                    item = db.WithdrawalApplications.First();
                    Assert.DoesNotThrow(() => repository.ApproveWithdrawal(item));
                    Assert.True(item != null && item.IsApproved);
                }
            }
            catch (Exception ex)
            {
                LogEventManager.Logger.Error(ex.Message, ex);
                throw;
            }
        }
    }
}
