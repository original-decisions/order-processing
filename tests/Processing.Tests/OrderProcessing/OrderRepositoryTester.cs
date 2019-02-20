using System;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using NUnit.Framework;
using odec.Framework.Generic.Utility;
using odec.Framework.Logging;
using odec.Processing.DAL;
using odec.Server.Model.Contact;
using odec.Server.Model.OrderProcessing;
using odec.Server.Model.OrderProcessing.Contexts;
using odec.Server.Model.OrderProcessing.Filters;
using odec.Server.Model.User;

namespace Processing.Tests.OrderProcessing
{
    public class OrderRepositoryTester : Tester<OrderContext>
    {
        [Test]
        public void SaveById()
        {
            try
            {
                var options = CreateNewContextOptions();
                Contact contact = ProcessingTestHelper.GenerateContact();
                using (var db = new OrderContext(options))
                {
                    db.Contacts.Add(contact);
                    ProcessingTestHelper.PopulateDefaultOrderCtx(db);

                }

                using (var db = new OrderContext(options))
                {
                    var repository =
                        new OrderRepository(db);
                    var orderState = db.OrderStates.First();
                    var payment = db.PaymentMethods.First();
                    var delivery = db.DeliveryMethods.First();
                    var zone = db.DeliveryZones.First();
                    var item = ProcessingTestHelper.GenerateOrder(contact, orderState, payment, delivery, zone);
                    Assert.DoesNotThrow(() => repository.SaveById(item));
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
        public void Save()
        {
            try
            {
                var options = CreateNewContextOptions();
                Contact contact = ProcessingTestHelper.GenerateContact();
                using (var db = new OrderContext(options))
                {
                    db.Contacts.Add(contact);
                    ProcessingTestHelper.PopulateDefaultOrderCtx(db);
                }
                using (var db = new OrderContext(options))
                {
                    var repository =
                        new OrderRepository(db);

                    var orderState = db.OrderStates.First();
                    var payment = db.PaymentMethods.First();
                    var delivery = db.DeliveryMethods.First();
                    var zone = db.DeliveryZones.First();
                    var item = ProcessingTestHelper.GenerateOrder(contact, orderState, payment, delivery, zone);
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
                Contact contact = ProcessingTestHelper.GenerateContact();
                using (var db = new OrderContext(options))
                {
                    db.Contacts.Add(contact);
                    ProcessingTestHelper.PopulateDefaultOrderCtx(db);
                }
                using (var db = new OrderContext(options))
                {
                    var repository = new OrderRepository(db);

                    var orderState = db.OrderStates.First();
                    var payment = db.PaymentMethods.First();
                    var delivery = db.DeliveryMethods.First();
                    var zone = db.DeliveryZones.First();
                    var item = ProcessingTestHelper.GenerateOrder(contact, orderState, payment, delivery, zone);
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
                Contact contact = ProcessingTestHelper.GenerateContact();
                using (var db = new OrderContext(options))
                {
                    db.Contacts.Add(contact);
                    ProcessingTestHelper.PopulateDefaultOrderCtx(db);
                }
                using (var db = new OrderContext(options))
                {
                    var repository = new OrderRepository(db);

                    var orderState = db.OrderStates.First();
                    var payment = db.PaymentMethods.First();
                    var delivery = db.DeliveryMethods.First();
                    var zone = db.DeliveryZones.First();
                    var item = ProcessingTestHelper.GenerateOrder(contact, orderState, payment, delivery, zone);
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
        public void Deactivate()
        {
            try
            {
                var options = CreateNewContextOptions();
                Contact contact = ProcessingTestHelper.GenerateContact();
                using (var db = new OrderContext(options))
                {
                    db.Contacts.Add(contact);
                    ProcessingTestHelper.PopulateDefaultOrderCtx(db);
                }
                using (var db = new OrderContext(options))
                {
                    var repository = new OrderRepository(db);

                    var orderState = db.OrderStates.First();
                    var payment = db.PaymentMethods.First();
                    var delivery = db.DeliveryMethods.First();
                    var zone = db.DeliveryZones.First();
                    var item = ProcessingTestHelper.GenerateOrder(contact, orderState, payment, delivery, zone);
                    item.IsActive = true;
                    Assert.DoesNotThrow(() => repository.Save(item));
                    Assert.DoesNotThrow(() => repository.Deactivate(item));
                    Assert.DoesNotThrow(() => repository.Delete(item));
                    Assert.IsFalse(item.IsActive);
                }

            }
            catch (Exception ex)
            {
                LogEventManager.Logger.Error(ex);
                throw;
            }
        }

        [Test]
        public void DeactivateById()
        {
            try
            {
                var options = CreateNewContextOptions();
                Contact contact = ProcessingTestHelper.GenerateContact();
                using (var db = new OrderContext(options))
                {
                    db.Contacts.Add(contact);
                    ProcessingTestHelper.PopulateDefaultOrderCtx(db);
                }
                using (var db = new OrderContext(options))
                {
                    var repository = new OrderRepository(db);

                    var orderState = db.OrderStates.First();
                    var payment = db.PaymentMethods.First();
                    var delivery = db.DeliveryMethods.First();
                    var zone = db.DeliveryZones.First();
                    var item = ProcessingTestHelper.GenerateOrder(contact, orderState, payment, delivery, zone);
                    item.IsActive = true;
                    Assert.DoesNotThrow(() => repository.Save(item));
                    Assert.DoesNotThrow(() => item = repository.Deactivate(item.Id));
                    Assert.DoesNotThrow(() => repository.Delete(item));
                    Assert.IsFalse(item.IsActive);
                }

            }
            catch (Exception ex)
            {
                LogEventManager.Logger.Error(ex);
                throw;
            }
        }

        [Test]
        public void Activate()
        {
            try
            {
                var options = CreateNewContextOptions();
                Contact contact = ProcessingTestHelper.GenerateContact();
                using (var db = new OrderContext(options))
                {
                    db.Contacts.Add(contact);
                    ProcessingTestHelper.PopulateDefaultOrderCtx(db);
                }
                using (var db = new OrderContext(options))
                {
                    var repository = new OrderRepository(db);

                    var orderState = db.OrderStates.First();
                    var payment = db.PaymentMethods.First();
                    var delivery = db.DeliveryMethods.First();
                    var zone = db.DeliveryZones.First();
                    var item = ProcessingTestHelper.GenerateOrder(contact, orderState, payment, delivery, zone);
                    item.IsActive = false;
                    Assert.DoesNotThrow(() => repository.Save(item));
                    Assert.DoesNotThrow(() => repository.Activate(item));
                    Assert.DoesNotThrow(() => repository.Delete(item));
                    Assert.IsTrue(item.IsActive);
                }

            }
            catch (Exception ex)
            {
                LogEventManager.Logger.Error(ex);
                throw;
            }
        }

        [Test]
        public void ActivateById()
        {
            try
            {
                var options = CreateNewContextOptions();
                Contact contact = ProcessingTestHelper.GenerateContact();
                using (var db = new OrderContext(options))
                {
                    db.Contacts.Add(contact);
                    ProcessingTestHelper.PopulateDefaultOrderCtx(db);
                }
                using (var db = new OrderContext(options))
                {
                    var repository = new OrderRepository(db);


                    var orderState = db.OrderStates.First();
                    var payment = db.PaymentMethods.First();
                    var delivery = db.DeliveryMethods.First();
                    var zone = db.DeliveryZones.First();
                    var item = ProcessingTestHelper.GenerateOrder(contact, orderState, payment, delivery, zone);
                    item.IsActive = false;
                    Assert.DoesNotThrow(() => repository.Save(item));
                    Assert.DoesNotThrow(() => item = repository.Activate(item.Id));
                    Assert.DoesNotThrow(() => repository.Delete(item));
                    Assert.IsTrue(item.IsActive);
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
                Contact contact = ProcessingTestHelper.GenerateContact();
                using (var db = new OrderContext(options))
                {
                    db.Contacts.Add(contact);
                    ProcessingTestHelper.PopulateDefaultOrderCtx(db);
                }
                using (var db = new OrderContext(options))
                {
                    var repository = new OrderRepository(db);

                    var orderState = db.OrderStates.First();
                    var payment = db.PaymentMethods.First();
                    var delivery = db.DeliveryMethods.First();
                    var zone = db.DeliveryZones.First();
                    var item = ProcessingTestHelper.GenerateOrder(contact, orderState, payment, delivery, zone);
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

        class GetOrdersTestCases : IEnumerable
        {
            public IEnumerator GetEnumerator()
            {
                var options = Tester<OrderContext>.CreateNewContextOptions();
                var contact = ProcessingTestHelper.GenerateContact();
                User user = new User
                {
                    UserName = "Andrew"
                };
                OrderState orderState1;
                OrderState orderState2;
                PaymentMethod payment1;
                PaymentMethod payment2;
                DeliveryMethod delivery1;
                DeliveryMethod delivery2;
                DeliveryZone zone;
                using (var db = new OrderContext(options))
                {
                    db.Contacts.Add(contact);
                    db.Set<User>().Add(user);
                    db.Set<UserContact>().Add(new UserContact
                    {
                        IsAccountBased = true,
                        UserId = user.Id,
                        ContactId = contact.Id
                    });
                    ProcessingTestHelper.PopulateDefaultOrderCtx(db);
                    orderState1 = db.OrderStates.First();
                    orderState2 = db.OrderStates.Last();
                    payment1 = db.PaymentMethods.First();
                    payment2 = db.PaymentMethods.Last();
                    delivery1 = db.DeliveryMethods.First();
                    delivery2 = db.DeliveryMethods.Last();
                    zone = db.DeliveryZones.First();
                    var order1 = ProcessingTestHelper.GenerateOrder(contact, orderState1, payment1, delivery1, zone);
                    var order2 = ProcessingTestHelper.GenerateOrder(contact, orderState1, payment1, delivery1, zone);
                    var order3 = ProcessingTestHelper.GenerateOrder(contact, orderState1, payment1, delivery1, zone);
                    var order4 = ProcessingTestHelper.GenerateOrder(contact, orderState2, payment2, delivery2, zone);
                    var order5 = ProcessingTestHelper.GenerateOrder(contact, orderState2, payment2, delivery2, zone);
                    var order6 = ProcessingTestHelper.GenerateOrder(ProcessingTestHelper.GenerateContact(), orderState2, payment2, delivery2, zone);
                    db.Set<Order>().AddRange(order1, order2, order3, order4, order5, order6);
                    db.SaveChanges();
                }
                //Order is required so result is 0
                var emptyFilter = new OrderFilter<int?>();
                var serializerSettings = new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore
                };



                var userFilter = new OrderFilter<int?>
                {
                    UserId = user.Id
                };
                var userDeliveryFilter1 = new OrderFilter<int?>
                {
                    UserId = user.Id,
                    DeliveryMethodId = delivery2.Id
                };
                var userDeliveryFilter2 = new OrderFilter<int?>
                {
                    UserId = user.Id,
                    DeliveryMethodId = delivery1.Id
                };
                var deliveryFilter = new OrderFilter<int?>
                {
                    DeliveryMethodId = delivery2.Id
                };

                var deliveryPaymentUserFilter = new OrderFilter<int?>
                {
                    UserId = user.Id,
                    DeliveryMethodId = delivery1.Id,
                    PaymentMethodId = payment1.Id,

                };

                var deliveryPaymentStateUserFilter = new OrderFilter<int?>
                {
                    UserId = user.Id,
                    DeliveryMethodId = delivery1.Id,
                    PaymentMethodId = payment1.Id,
                    OrderStateId = orderState1.Id
                };

                var deliveryPaymentStateUserFilter2 = new OrderFilter<int?>
                {
                    UserId = user.Id,
                    DeliveryMethodId = delivery1.Id,
                    PaymentMethodId = payment2.Id,
                    OrderStateId = orderState1.Id
                };

                var deliveryPaymentStateUserFilter3 = new OrderFilter<int?>
                {
                    UserId = user.Id,
                    DeliveryMethodId = delivery1.Id,
                    PaymentMethodId = payment1.Id,
                    OrderStateId = orderState2.Id
                };

                var deliveryPaymentStateUserFilter4 = new OrderFilter<int?>
                {
                    UserId = user.Id,
                    DeliveryMethodId = delivery2.Id,
                    PaymentMethodId = payment1.Id,
                    OrderStateId = orderState1.Id
                };

                var deliveryPaymentUserFilter2 = new OrderFilter<int?>
                {
                    UserId = user.Id,
                    DeliveryMethodId = delivery2.Id,
                    PaymentMethodId = payment1.Id
                };
                var deliveryPaymentUserFilter3 = new OrderFilter<int?>
                {
                    UserId = user.Id,
                    DeliveryMethodId = delivery1.Id,
                    PaymentMethodId = payment2.Id
                };
                var userTotalFilter = new OrderFilter<int?>
                {
                    UserId = user.Id,
                    TotalInterval = new Interval<decimal?> { Start = 2001 }
                };
                var totalStartFilter = new OrderFilter<int?>
                {
                    TotalInterval = new Interval<decimal?> { Start = 2001 }
                };
                var totalFilter = new OrderFilter<int?>
                {
                    TotalInterval = new Interval<decimal?> { Start = 2001, End = 2004 }
                };
                var totalFilter1 = new OrderFilter<int?>
                {
                    TotalInterval = new Interval<decimal?> { Start = 500, End = 1999 }
                };
                var totalFilterEnd = new OrderFilter<int?>
                {
                    TotalInterval = new Interval<decimal?> { End = 1999 }
                };
                var enumerator = new List<Tuple<OrderFilter<int?>, int>>
                {
                    new Tuple<OrderFilter<int?>, int>(emptyFilter, 6),
                    new Tuple<OrderFilter<int?>, int>(userFilter, 5),
                    new Tuple<OrderFilter<int?>, int>(deliveryFilter, 3),
                    new Tuple<OrderFilter<int?>, int>(userDeliveryFilter1, 2),
                    new Tuple<OrderFilter<int?>, int>(userDeliveryFilter2, 3),
                    new Tuple<OrderFilter<int?>, int>(deliveryPaymentUserFilter, 3),


                    new Tuple<OrderFilter<int?>, int>(deliveryPaymentStateUserFilter, 3),
                    new Tuple<OrderFilter<int?>, int>(deliveryPaymentStateUserFilter2, 0),
                    new Tuple<OrderFilter<int?>, int>(deliveryPaymentStateUserFilter3, 0),
                    new Tuple<OrderFilter<int?>, int>(deliveryPaymentStateUserFilter4, 0),
                    new Tuple<OrderFilter<int?>, int>(deliveryPaymentUserFilter2, 0),
                    new Tuple<OrderFilter<int?>, int>(deliveryPaymentUserFilter3, 0),
                    new Tuple<OrderFilter<int?>, int>(userTotalFilter, 0),
                     new Tuple<OrderFilter<int?>, int>(totalStartFilter, 0),
                      new Tuple<OrderFilter<int?>, int>(totalFilter, 0),
   new Tuple<OrderFilter<int?>, int>(totalFilter1, 0),
      new Tuple<OrderFilter<int?>, int>(totalFilterEnd, 0),


                };
                return
                    enumerator.Select(
                            it =>
                                new TestCaseData(options, it.Item1, it.Item2)
                                    .SetName("Get: Filter -> " +
                                             JsonConvert.SerializeObject(it.Item1, serializerSettings).Replace('.', ',')
                                                 ))
                        .GetEnumerator();
            }
        }
        [Test]
        [TestCaseSource(typeof(GetOrdersTestCases))]
        public void Get(DbContextOptions<OrderContext> options, OrderFilter<int?> filter, int expectedResult)
        {
            try
            {
                using (var db = new OrderContext(options))
                {
                    var repository =
                        new OrderRepository(db);
                    IEnumerable<Order> result = null;
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
        public void AddOrderDetails()
        {
            try
            {
                var options = Tester<OrderContext>.CreateNewContextOptions();
                var orderDetails = new List<OrderDetail>
                {
                    new OrderDetail
                    {
                        DiscountedCost = 200,
                        EntityCount = 20,
                        Total = 4000,
                        JsonEntityDetails = JsonConvert.SerializeObject(new  {Id=1,Name="Hat",IsActive=true, Cost=200 }),
                        MomentCost = 200
                    },
                    new OrderDetail
                    {
                        DiscountedCost = 300,
                        EntityCount = 20,
                        Total = 4000,
                        JsonEntityDetails = JsonConvert.SerializeObject(new  {Id=2,Name="Hat2",IsActive=true, Cost=300 }),
                        MomentCost = 300
                    },
                };
                var contact = ProcessingTestHelper.GenerateContact();
                Order order;
                using (var db = new OrderContext(options))
                {
                    db.Contacts.Add(contact);
                    ProcessingTestHelper.PopulateDefaultOrderCtx(db);
                    var orderState1 = db.OrderStates.First();
                    var payment1 = db.PaymentMethods.First();
                    var delivery1 = db.DeliveryMethods.First();
                    var zone = db.DeliveryZones.First();
                    order = ProcessingTestHelper.GenerateOrder(contact, orderState1, payment1, delivery1, zone);
                    db.Set<Order>().Add(order);
                    db.SaveChanges();
                }
                using (var db = new OrderContext(options))
                {
                    var repository =
                        new OrderRepository(db);
                    Assert.DoesNotThrow(() => repository.AddOrderDetails(orderDetails, order));
                    Assert.True(orderDetails.All(it => it.OrderId != 0));
                }
            }
            catch (Exception ex)
            {
                LogEventManager.Logger.Error(ex.Message, ex);
                throw;
            }
        }
        [Test]
        [TestCaseSource(typeof(GetOrderDetailsTestCases))]
        public void GetOrderDetails(DbContextOptions<OrderContext> options, OrderDetailsFilter<int> filter, int expectedResult)
        {
            try
            {
                using (var db = new OrderContext(options))
                {
                    var repository =
                        new OrderRepository(db);
                    IEnumerable<OrderDetail> result = null;
                    Assert.DoesNotThrow(() => result = repository.GetOrderDetails(filter));
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
        public void GetOrderDetailsByOrder()
        {
            try
            {
                var options = Tester<OrderContext>.CreateNewContextOptions();
                var contact = ProcessingTestHelper.GenerateContact();
                Order order;
                using (var db = new OrderContext(options))
                {
                    db.Contacts.Add(contact);
                    ProcessingTestHelper.PopulateDefaultOrderCtx(db);
                    var orderState1 = db.OrderStates.First();
                    var payment1 = db.PaymentMethods.First();
                    var delivery1 = db.DeliveryMethods.First();
                    var zone = db.DeliveryZones.First();
                    order = ProcessingTestHelper.GenerateOrder(contact, orderState1, payment1, delivery1, zone);
                    db.Set<Order>().Add(order);
                    var detailsOrder = ProcessingTestHelper.GenerateOrderDetail(order,
                    new { Id = 1, Name = "Hat", Cost = 200 });
                    var detailsOrder2 = ProcessingTestHelper.GenerateOrderDetail(order,
                        new { Id = 2, Name = "Hat", Cost = 200 });
                    var detailsOrder3 = ProcessingTestHelper.GenerateOrderDetail(order,
                        new { Id = 3, Name = "Hat", Cost = 200 });
                    var detailsOrder4 = ProcessingTestHelper.GenerateOrderDetail(order,
                       new { Id = 4, Name = "Hat", Cost = 200 });
                    var detailsOrder5 = ProcessingTestHelper.GenerateOrderDetail(order,
                       new { Id = 5, Name = "Hat", Cost = 200 });
                    db.Set<OrderDetail>().AddRange(detailsOrder, detailsOrder2, detailsOrder3, detailsOrder4, detailsOrder5);
                    db.SaveChanges();
                }

                using (var db = new OrderContext(options))
                {
                    var repository =
                        new OrderRepository(db);
                    IEnumerable<OrderDetail> result = null;
                    Assert.DoesNotThrow(() => result = repository.GetOrderDetails(order));
                    Assert.True(result != null && result.Count() == 5);
                }
            }
            catch (Exception ex)
            {
                LogEventManager.Logger.Error(ex.Message, ex);
                throw;
            }
        }
        [Test]
        public void ChangeState()
        {
            try
            {
                var options = Tester<OrderContext>.CreateNewContextOptions();
                var contact = ProcessingTestHelper.GenerateContact();
                Order order;
                OrderState resultState;
                using (var db = new OrderContext(options))
                {
                    db.Contacts.Add(contact);
                    ProcessingTestHelper.PopulateDefaultOrderCtx(db);
                    var orderState1 = db.OrderStates.First();
                    var payment1 = db.PaymentMethods.First();
                    var delivery1 = db.DeliveryMethods.First();
                    var zone = db.DeliveryZones.First();
                    resultState = db.OrderStates.Last();
                    order = ProcessingTestHelper.GenerateOrder(contact, orderState1, payment1, delivery1, zone);
                    db.Set<Order>().Add(order);
                    db.SaveChanges();
                }

                using (var db = new OrderContext(options))
                {
                    var repository =
                        new OrderRepository(db);
                    Assert.DoesNotThrow(() => repository.ChangeState(order.Id, resultState));
                    order = repository.GetById(order.Id);
                    Assert.True(order.OrderStateId == resultState.Id);
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
