using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using odec.Entity.DAL;
using odec.Framework.Extensions;
using odec.Framework.Logging;
using odec.Processing.DAL.Interop;
using odec.Server.Model.OrderProcessing;
using odec.Server.Model.OrderProcessing.Filters;

namespace odec.Processing.DAL
{
    public class DeliveryMethodRepository : OrmEntityOperationsRepository<int, DeliveryMethod, DbContext>, IDeliveryRepository<int, DbContext, DeliveryMethod, DeliveryZone, DeliveryMethodFilter>
    {
        public IEnumerable<DeliveryMethod> Get(DeliveryMethodFilter filter)
        {
            try
            {
                var query = Db.Set<DeliveryMethod>().AsQueryable();
                //TODO: dependencies.
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

        public IEnumerable<DeliveryZone> GetDeliveryZones(DeliveryMethod deliveryMethod)
        {
            try
            {
                return from charge in Db.Set<DeliveryCharge>()
                       join deliveryZone in Db.Set<DeliveryZone>() on charge.ZoneId equals deliveryZone.Id
                       where charge.DeliveryMethodId == deliveryMethod.Id
                       select deliveryZone;
            }
            catch (Exception ex)
            {
                LogEventManager.Logger.Error(ex.Message, ex);
                throw;
            }
        }

        public decimal GetDeliveryCharge(DeliveryMethod deliveryMethod, DeliveryZone zone)
        {
            try
            {
                return
                    Db.Set<DeliveryCharge>()
                        .First(it => it.DeliveryMethodId == deliveryMethod.Id && it.ZoneId == zone.Id)
                        .ChargeValue;
            }
            catch (Exception ex)
            {
                LogEventManager.Logger.Error(ex.Message, ex);
                throw;
            }
        }

        public DeliveryMethodRepository() { }

        public DeliveryMethodRepository(DbContext db)
        {
            Db = db;
        }
        public void SetConnection(string connection)
        {
            throw new NotImplementedException();
        }

        public void SetContext(DbContext db)
        {
            Db = db;
        }
    }
}
