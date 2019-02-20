using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using odec.Framework.Extensions;
using odec.Framework.Generic.Utility;
using odec.Framework.Logging;
using odec.Processing.DAL;
using odec.Server.Model.Contact;
using odec.Server.Model.OrderProcessing;
using odec.Server.Model.OrderProcessing.Contexts;
using odec.Server.Model.OrderProcessing.Filters;

namespace Processing.Tests.OrderProcessing
{
    public class GetOrderDetailsTestCases : IEnumerable
    {
        public IEnumerator GetEnumerator()
        {
            var options = Tester<OrderContext>.CreateNewContextOptions();
            var contact = ProcessingTestHelper.GenerateContact();
            Order order1, order2;
            using (var db = new OrderContext(options))
            {
                db.Contacts.Add(contact);
                ProcessingTestHelper.PopulateDefaultOrderCtx(db);
                var orderState = db.OrderStates.First();
                var payment = db.PaymentMethods.First();
                var delivery = db.DeliveryMethods.First();
                var zone = db.DeliveryZones.First();
                order1 = ProcessingTestHelper.GenerateOrder(contact, orderState, payment, delivery, zone);
                order2 = ProcessingTestHelper.GenerateOrder(contact, orderState, payment, delivery, zone);
                db.Set<Order>().AddRange(order1, order2);
                var detailsOrder = ProcessingTestHelper.GenerateOrderDetail(order1,
                    new { Id = 1, Name = "Hat", Cost = 200 });
                var detailsOrder2 = ProcessingTestHelper.GenerateOrderDetail(order1,
                    new { Id = 2, Name = "Hat", Cost = 200 });
                var detailsOrder3 = ProcessingTestHelper.GenerateOrderDetail(order1,
                    new { Id = 3, Name = "Hat", Cost = 200 });
                var detailsOrder4 = ProcessingTestHelper.GenerateOrderDetail(order2,
                   new { Id = 4, Name = "Hat", Cost = 200 });
                var detailsOrder5 = ProcessingTestHelper.GenerateOrderDetail(order2,
                   new { Id = 5, Name = "Hat", Cost = 200 });
                db.Set<OrderDetail>().AddRange(detailsOrder, detailsOrder2, detailsOrder3, detailsOrder4, detailsOrder5);
                db.SaveChanges();
            }
            //Order is required so result is 0
            yield return new TestCaseData(options, new OrderDetailsFilter<int>(), 0);
            yield return new TestCaseData(options, new OrderDetailsFilter<int>
            {
                OrderId = order1.Id
            }, 3);
            yield return new TestCaseData(options, new OrderDetailsFilter<int>
            {
                OrderId = order2.Id
            }, 2);
            yield return new TestCaseData(options, new OrderDetailsFilter<int>
            {
                OrderId = order1.Id,
                TotalInterval = new Interval<decimal?> { Start = 999 }
            }, 3);
            yield return new TestCaseData(options, new OrderDetailsFilter<int>
            {
                OrderId = order1.Id,
                TotalInterval = new Interval<decimal?> { Start = 888, End = 1001 }
            }, 3);
            yield return new TestCaseData(options, new OrderDetailsFilter<int>
            {
                OrderId = order1.Id,
                TotalInterval = new Interval<decimal?> { Start = 1001 }
            }, 0);
            yield return new TestCaseData(options, new OrderDetailsFilter<int>
            {
                OrderId = order1.Id,
                TotalInterval = new Interval<decimal?> { Start = 1001, End = 1004 }
            }, 0);
            yield return new TestCaseData(options, new OrderDetailsFilter<int>
            {
                OrderId = order1.Id,
                TotalInterval = new Interval<decimal?> { Start = 500, End = 999 }
            }, 0);
            yield return new TestCaseData(options, new OrderDetailsFilter<int>
            {
                OrderId = order1.Id,
                TotalInterval = new Interval<decimal?> { End = 999 }
            }, 0);
        }
    }
    public class OrderDetailsRepositoryTester : Tester<OrderContext>
    {
        

        [Test]
        [TestCaseSource(typeof(GetOrderDetailsTestCases))]

        public void Get(DbContextOptions<OrderContext> options, OrderDetailsFilter<int> filter, int expectedResult)
        {
            try
            {

                using (var db = new OrderContext(options))
                {
                    var repository =
                        new OrderDetailsRepository(db);
                    IEnumerable<OrderDetail> result = null;
                    Assert.DoesNotThrow(() => result = repository.Get(filter));
                    Assert.True(result != null && result.Count() == expectedResult);
                }

            }
            catch (Exception ex)
            {
                LogEventManager.Logger.Error(ex.Message, ex);
                throw;
            }
        }
        [Test]
        public void GetByOrder()
        {
            try
            {
                var options = CreateNewContextOptions();
                var contact = ProcessingTestHelper.GenerateContact();
                Order order1;
                using (var db = new OrderContext(options))
                {
                    db.Contacts.Add(contact);
                    ProcessingTestHelper.PopulateDefaultOrderCtx(db);
                    var orderState = db.OrderStates.First();
                    var payment = db.PaymentMethods.First();
                    var delivery = db.DeliveryMethods.First();
                    var zone = db.DeliveryZones.First();
                    order1 = ProcessingTestHelper.GenerateOrder(contact, orderState, payment, delivery, zone);
                    
                    db.Set<Order>().AddRange(order1);
                    var detailsOrder = ProcessingTestHelper.GenerateOrderDetail(order1,
                        new { Id = 1, Name = "Hat", Cost = 200 });
                    var detailsOrder2 = ProcessingTestHelper.GenerateOrderDetail(order1,
                        new { Id = 2, Name = "Hat", Cost = 200 });
                    var detailsOrder3 = ProcessingTestHelper.GenerateOrderDetail(order1,
                        new { Id = 3, Name = "Hat", Cost = 200 });
                    db.Set<OrderDetail>().AddRange(detailsOrder, detailsOrder2, detailsOrder3);
                    db.SaveChanges();
                }
                using (var db = new OrderContext(options))
                {
                    var repository =
                        new OrderDetailsRepository(db);
                    IEnumerable<OrderDetail> result = null;
                    Assert.DoesNotThrow(() => result = repository.GetByOrder(order1.Id));
                    Assert.True(result != null && result.Count() == 3);
                }
            }
            catch (Exception ex)
            {
                LogEventManager.Logger.Error(ex.Message, ex);
                throw;
            }
        }


        [Test]
        public void Save()
        {
            try
            {
                var options = CreateNewContextOptions();
                Order order;
                var obj = new
                {
                    Id = 1,
                    Name = "Hat",
                    Price = 200
                };
                using (var db = new OrderContext(options))
                {
                    var contact = ProcessingTestHelper.GenerateContact();
                    ProcessingTestHelper.PopulateDefaultOrderCtx(db);
                    var orderState = db.OrderStates.First();
                    var payment = db.PaymentMethods.First();
                    var delivery = db.DeliveryMethods.First();
                    var zone = db.DeliveryZones.First();
                    order = ProcessingTestHelper.GenerateOrder(contact, orderState, payment, delivery, zone);
                    db.Orders.Add(order);
                    db.SaveChanges();
                }
                using (var db = new OrderContext(options))
                {
                    var repository =
                        new OrderDetailsRepository(db);

                    var item = ProcessingTestHelper.GenerateOrderDetail(order, obj);
                    Assert.DoesNotThrow(() => repository.Save(item));
                    Assert.DoesNotThrow(() => repository.Delete(item));
                    Assert.Greater(item.Id, 0);
                }
            }
            catch (Exception ex)
            {
                LogEventManager.Logger.Error(ex);
                throw;
            }
        }

        [Test]
        public void Delete()
        {
            try
            {
                var options = CreateNewContextOptions();
                Order order;
                var obj = new
                {
                    Id = 1,
                    Name = "Hat",
                    Price = 200
                };
                using (var db = new OrderContext(options))
                {
                    var contact = ProcessingTestHelper.GenerateContact();
                    ProcessingTestHelper.PopulateDefaultOrderCtx(db);
                    var orderState = db.OrderStates.First();
                    var payment = db.PaymentMethods.First();
                    var delivery = db.DeliveryMethods.First();
                    var zone = db.DeliveryZones.First();
                    order = ProcessingTestHelper.GenerateOrder(contact, orderState, payment, delivery, zone);
                    db.Orders.Add(order);
                    db.SaveChanges();
                }
                using (var db = new OrderContext(options))
                {
                    var repository = new OrderDetailsRepository(db);

                    var item = ProcessingTestHelper.GenerateOrderDetail(order, obj);
                    Assert.DoesNotThrow(() => repository.Save(item));
                    Assert.DoesNotThrow(() => repository.Delete(item));
                }

            }
            catch (Exception ex)
            {
                LogEventManager.Logger.Error(ex);
                throw;
            }
        }

        [Test]
        public void DeleteById()
        {
            try
            {
                var options = CreateNewContextOptions();
                Order order;
                var obj = new
                {
                    Id = 1,
                    Name = "Hat",
                    Price = 200
                };
                using (var db = new OrderContext(options))
                {
                    var contact = ProcessingTestHelper.GenerateContact();
                    ProcessingTestHelper.PopulateDefaultOrderCtx(db);
                    var orderState = db.OrderStates.First();
                    var payment = db.PaymentMethods.First();
                    var delivery = db.DeliveryMethods.First();
                    var zone = db.DeliveryZones.First();
                    order = ProcessingTestHelper.GenerateOrder(contact, orderState, payment, delivery, zone);
                    db.Orders.Add(order);
                    db.SaveChanges();
                }
                using (var db = new OrderContext(options))
                {
                    var repository = new OrderDetailsRepository(db);

                    var item = ProcessingTestHelper.GenerateOrderDetail(order, obj);
                    Assert.DoesNotThrow(() => repository.Save(item));
                    Assert.DoesNotThrow(() => repository.Delete(item.Id));
                }

            }
            catch (Exception ex)
            {
                LogEventManager.Logger.Error(ex);
                throw;
            }
        }

        [Test]
        public void GetById()
        {
            try
            {
                var options = CreateNewContextOptions();
                Order order;
                var obj = new
                {
                    Id = 1,
                    Name = "Hat",
                    Price = 200
                };
                using (var db = new OrderContext(options))
                {
                    var contact = ProcessingTestHelper.GenerateContact();
                    ProcessingTestHelper.PopulateDefaultOrderCtx(db);
                    var orderState = db.OrderStates.First();
                    var payment = db.PaymentMethods.First();
                    var delivery = db.DeliveryMethods.First();
                    var zone = db.DeliveryZones.First();
                    order = ProcessingTestHelper.GenerateOrder(contact, orderState, payment, delivery, zone);
                    db.Orders.Add(order);
                    db.SaveChanges();
                }
                using (var db = new OrderContext(options))
                {
                    var repository = new OrderDetailsRepository(db);

                    var item = ProcessingTestHelper.GenerateOrderDetail(order, obj);
                    Assert.DoesNotThrow(() => repository.Save(item));

                    Assert.DoesNotThrow(() => item = repository.GetById(item.Id));
                    Assert.DoesNotThrow(() => repository.Delete(item));
                    Assert.NotNull(item);
                    Assert.Greater(item.Id, 0);
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
