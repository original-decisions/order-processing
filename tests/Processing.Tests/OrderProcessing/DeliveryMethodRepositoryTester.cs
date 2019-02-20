using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using odec.Framework.Logging;
using odec.Processing.DAL;
using odec.Server.Model.OrderProcessing;
using odec.Server.Model.OrderProcessing.Contexts;
using odec.Server.Model.OrderProcessing.Filters;

namespace Processing.Tests.OrderProcessing
{
    public class DeliveryMethodRepositoryTester : Tester<OrderContext>
    {
        [Test]
        public void SaveById()
        {
            try
            {
                var options = CreateNewContextOptions();
                using (var db = new OrderContext(options))
                {
                    ProcessingTestHelper.PopulateDefaultOrderCtx(db);
                }
                using (var db = new OrderContext(options))
                {
                    var repository =
                        new DeliveryMethodRepository(db);

                    var item = ProcessingTestHelper.GenerateDeliveryMethod();
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
                using (var db = new OrderContext(options))
                {
                    ProcessingTestHelper.PopulateDefaultOrderCtx(db);
                }
                using (var db = new OrderContext(options))
                {
                    var repository =
                        new DeliveryMethodRepository(db);

                    var item = ProcessingTestHelper.GenerateDeliveryMethod();
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
                using (var db = new OrderContext(options))
                {
                    ProcessingTestHelper.PopulateDefaultOrderCtx(db);
                }
                using (var db = new OrderContext(options))
                {
                    var repository = new DeliveryMethodRepository(db);

                    var item = ProcessingTestHelper.GenerateDeliveryMethod();
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
                using (var db = new OrderContext(options))
                {
                    ProcessingTestHelper.PopulateDefaultOrderCtx(db);
                }
                using (var db = new OrderContext(options))
                {
                    var repository = new DeliveryMethodRepository(db);

                    var item = ProcessingTestHelper.GenerateDeliveryMethod();
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
                using (var db = new OrderContext(options))
                {
                    ProcessingTestHelper.PopulateDefaultOrderCtx(db);
                }
                using (var db = new OrderContext(options))
                {
                    var repository = new DeliveryMethodRepository(db);

                    var item = ProcessingTestHelper.GenerateDeliveryMethod();
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
                using (var db = new OrderContext(options))
                {
                    ProcessingTestHelper.PopulateDefaultOrderCtx(db);
                }
                using (var db = new OrderContext(options))
                {
                    var repository = new DeliveryMethodRepository(db);

                    var item = ProcessingTestHelper.GenerateDeliveryMethod();
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
                using (var db = new OrderContext(options))
                {
                    ProcessingTestHelper.PopulateDefaultOrderCtx(db);
                }
                using (var db = new OrderContext(options))
                {
                    var repository = new DeliveryMethodRepository(db);

                    var item = ProcessingTestHelper.GenerateDeliveryMethod();
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
                using (var db = new OrderContext(options))
                {
                    ProcessingTestHelper.PopulateDefaultOrderCtx(db);
                }
                using (var db = new OrderContext(options))
                {
                    var repository = new DeliveryMethodRepository(db);


                    var item = ProcessingTestHelper.GenerateDeliveryMethod();
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
                using (var db = new OrderContext(options))
                {
                    ProcessingTestHelper.PopulateDefaultOrderCtx(db);
                }
                using (var db = new OrderContext(options))
                {
                    var repository = new DeliveryMethodRepository(db);

                    var item = ProcessingTestHelper.GenerateDeliveryMethod();
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
        [Test]
        [TestCase(1, 1, "Name", "asc", ExpectedResult = 1)]
        [TestCase(1, 2, "Name", "asc", ExpectedResult = 2)]
        [TestCase(2, 1, "Name", "asc", ExpectedResult = 1)]
        [TestCase(2, 1, "DateCreated", "desc", ExpectedResult = 1)]
        public int Get(int page, int rows, string sidx, string sord)
        {
            try
            {
                DeliveryMethodFilter filter = new DeliveryMethodFilter
                {
                    Page = page,
                    Rows = rows,
                    Sidx = sidx,
                    Sord = sord
                };
                var options = CreateNewContextOptions();
                using (var db = new OrderContext(options))
                {
                    ProcessingTestHelper.PopulateDefaultOrderCtx(db);
                }
                using (var db = new OrderContext(options))
                {
                    var repository = new DeliveryMethodRepository(db);
                    IEnumerable<DeliveryMethod> result = new List<DeliveryMethod>();
                    Assert.DoesNotThrow(() => result = repository.Get(filter));
                    return result.Count();
                }

            }
            catch (Exception ex)
            {
                LogEventManager.Logger.Error(ex.Message, ex);
                throw;
            }
        }
        [Test]
        [TestCase("BYCOURIER",ExpectedResult = 1)]
        [TestCase("POST", ExpectedResult = 0)]
        public int GetDeliveryZones(string deliveryMethodCode)
        {
            try
            {
                DeliveryMethod deliveryMethod;
                var options = CreateNewContextOptions();
                using (var db = new OrderContext(options))
                {
                    ProcessingTestHelper.PopulateDefaultOrderCtx(db);
                    deliveryMethod = db.DeliveryMethods.First(it => it.Code == deliveryMethodCode);
                }
                using (var db = new OrderContext(options))
                {
                    var repository = new DeliveryMethodRepository(db);
                    IEnumerable<DeliveryZone> result = new List<DeliveryZone>();
                    Assert.DoesNotThrow(() => result = repository.GetDeliveryZones(deliveryMethod));
                    return result.Count();
                }

            }
            catch (Exception ex)
            {
                LogEventManager.Logger.Error(ex.Message, ex);
                throw;
            }
        }
        [Test]
        [TestCase("BYCOURIER", "PECHATNIKI", ExpectedResult = 1000)]
        public decimal GetDeliveryCharge(string deliveryMethodCode, string zoneCode)
        {
            try
            {
                DeliveryMethod deliveryMethod;
                DeliveryZone deliveryZone;
                var options = CreateNewContextOptions();
                using (var db = new OrderContext(options))
                {
                    ProcessingTestHelper.PopulateDefaultOrderCtx(db);
                    deliveryMethod = db.DeliveryMethods.First(it => it.Code == deliveryMethodCode);
                    deliveryZone = db.DeliveryZones.First(it => it.Code == zoneCode);
                }
                using (var db = new OrderContext(options))
                {
                    var repository = new DeliveryMethodRepository(db);
                    decimal result = -1;
                    Assert.DoesNotThrow(() => result = repository.GetDeliveryCharge(deliveryMethod, deliveryZone));
                    return result;
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
