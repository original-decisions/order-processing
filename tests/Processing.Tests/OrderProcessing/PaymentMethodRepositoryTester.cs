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
    public class PaymentMethodRepositoryTester : Tester<OrderContext>
    {
        [Test]
        [TestCase(1,1,"Name","asc",ExpectedResult = 1)]
        [TestCase(1, 2, "Name", "asc", ExpectedResult = 2)]
        [TestCase(2, 1, "Name", "asc", ExpectedResult = 1)]
        [TestCase(2, 1, "DateCreated", "desc", ExpectedResult = 1)]
        public int Get(int page, int rows, string sidx, string sord)
        {
            try
            {
                PaymentMethodFilter filter = new PaymentMethodFilter
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
                    IEnumerable<PaymentMethod> result = new List<PaymentMethod>();
                    var repository = new PaymentMethodRepository(db);
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
                        new PaymentMethodRepository(db);

                    var item = ProcessingTestHelper.GeneratePaymentMethod();
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
                        new PaymentMethodRepository(db);
                   
                    var item = ProcessingTestHelper.GeneratePaymentMethod();
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
                    var repository = new PaymentMethodRepository(db);
                    
                    var item = ProcessingTestHelper.GeneratePaymentMethod();
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
                    var repository = new PaymentMethodRepository(db);
                    
                    var item = ProcessingTestHelper.GeneratePaymentMethod();
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
                    var repository = new PaymentMethodRepository(db);
                    
                    var item = ProcessingTestHelper.GeneratePaymentMethod();
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
                    var repository = new PaymentMethodRepository(db);
                    
                    var item = ProcessingTestHelper.GeneratePaymentMethod();
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
                    var repository = new PaymentMethodRepository(db);
                    
                    var item = ProcessingTestHelper.GeneratePaymentMethod();
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
                    var repository = new PaymentMethodRepository(db);

                    
                    var item = ProcessingTestHelper.GeneratePaymentMethod();
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
                    var repository = new PaymentMethodRepository(db);
                    
                    var item = ProcessingTestHelper.GeneratePaymentMethod();
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
