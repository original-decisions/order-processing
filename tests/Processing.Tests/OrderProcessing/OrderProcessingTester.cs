using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using odec.Framework.Logging;
using odec.Processing.DAL;
using odec.Server.Model.Contact;
using odec.Server.Model.OrderProcessing;
using odec.Server.Model.OrderProcessing.Contexts;

namespace Processing.Tests.OrderProcessing
{
    public class OrderProcessingTester : Tester<OrderContext>
    {


        [Test]
        public void ChangeOrderStateOrder()
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
                        new odec.Processing.DAL.OrderProcessing(db);
                    Assert.DoesNotThrow(() => repository.ChangeOrderState(order, resultState));
                    Assert.True(order.OrderStateId == resultState.Id);
                }
            }
            catch (Exception ex)
            {
                LogEventManager.Logger.Error(ex);
                throw;
            }
        }
        [Test]
        public void ChangeOrderStateById()
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
                        new odec.Processing.DAL.OrderProcessing(db);
                    Assert.DoesNotThrow(() => repository.ChangeOrderState(order.Id, resultState));
                    order = db.Set<Order>().Single(it => it.Id == order.Id);
                    Assert.True(order.OrderStateId == resultState.Id);
                }
            }
            catch (Exception ex)
            {
                LogEventManager.Logger.Error(ex);
                throw;
            }
        }
        [Test]
        public void GetAvailableStates()
        {
            try
            {
                var options = CreateNewContextOptions();
                Contact contact = ProcessingTestHelper.GenerateContact();
                Order item;
                using (var db = new OrderContext(options))
                {
                    db.Contacts.Add(contact);
                    ProcessingTestHelper.PopulateDefaultOrderCtx(db);
                    var orderState = db.OrderStates.First();
                    var payment = db.PaymentMethods.First();
                    var delivery = db.DeliveryMethods.First();
                    var zone = db.DeliveryZones.First();
                    item = ProcessingTestHelper.GenerateOrder(contact, orderState, payment, delivery, zone);
                    db.Set<Order>().Add(item);
                    db.SaveChanges();
                }
                using (var db = new OrderContext(options))
                {
                    var repository =
                        new odec.Processing.DAL.OrderProcessing(db);

                    IEnumerable<OrderState> result = null;
                    Assert.DoesNotThrow(() => result =repository.GetAvailableStates(item));
                    Assert.True(result!= null && result.Any());
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
