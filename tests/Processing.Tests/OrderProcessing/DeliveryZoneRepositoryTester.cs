using System;
using System.Linq;
using System.Collections;
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
    public class DeliveryZoneRepositoryTester : Tester<OrderContext>
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
                        new DeliveryZoneRepository(db);

                    var item = ProcessingTestHelper.GenerateDeliveryZone();
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
                        new DeliveryZoneRepository(db);

                    var item = ProcessingTestHelper.GenerateDeliveryZone();
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
                    var repository = new DeliveryZoneRepository(db);

                    var item = ProcessingTestHelper.GenerateDeliveryZone();
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
                    var repository = new DeliveryZoneRepository(db);

                    var item = ProcessingTestHelper.GenerateDeliveryZone();
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
                    var repository = new DeliveryZoneRepository(db);

                    var item = ProcessingTestHelper.GenerateDeliveryZone();
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
                    var repository = new DeliveryZoneRepository(db);

                    var item = ProcessingTestHelper.GenerateDeliveryZone();
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
                    var repository = new DeliveryZoneRepository(db);

                    var item = ProcessingTestHelper.GenerateDeliveryZone();
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
                    var repository = new DeliveryZoneRepository(db);


                    var item = ProcessingTestHelper.GenerateDeliveryZone();
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
                    var repository = new DeliveryZoneRepository(db);

                    var item = ProcessingTestHelper.GenerateDeliveryZone();
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

        class GetDeliveryZoneTestCases : IEnumerable
        {
            public IEnumerator GetEnumerator()
            {
                var options = CreateNewContextOptions();
                using (var db = new OrderContext(options))
                {
                    ProcessingTestHelper.PopulateDefaultOrderCtx(db);
                }

                yield return new TestCaseData(options, new DeliveryZoneFilter
                {
                    Page = 1,
                    Rows = 1,
                    Sidx = "Name",
                    Sord = "asc"
                }, 1);

                yield return new TestCaseData(options, new DeliveryZoneFilter
                {
                    Page = 1,
                    Sidx = "Name",
                    Sord = "asc"
                }, 5);
                yield return new TestCaseData(options, new DeliveryZoneFilter
                {
                    Page = 2,
                    Sidx = "Name",
                    Sord = "asc"
                }, 0);
                yield return new TestCaseData(options, new DeliveryZoneFilter
                {
                    Page = 1,
                    Rows = 2,
                    Sidx = "Name",
                    Sord = "asc"
                }, 2);
                yield return new TestCaseData(options, new DeliveryZoneFilter
                {
                    Page = 2,
                    Rows = 3,
                    Sidx = "Name",
                    Sord = "asc"
                }, 2);
            }
        }
        [Test]
        [TestCaseSource(typeof(GetDeliveryZoneTestCases))]
        public void Get(DbContextOptions<OrderContext> options, DeliveryZoneFilter filter, int expectedResult)
        {
            try
            {
                using (var db = new OrderContext(options))
                {
                    IEnumerable<DeliveryZone> result = new List<DeliveryZone>();
                    var repository = new DeliveryZoneRepository(db);
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
    }
}
