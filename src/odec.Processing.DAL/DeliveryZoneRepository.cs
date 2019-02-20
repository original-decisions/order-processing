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
    public class DeliveryZoneRepository : OrmEntityOperationsRepository<int, DeliveryZone, DbContext>, IDeliveryZoneRepository<int,DbContext,DeliveryZone,DeliveryZoneFilter>
    {
        public DeliveryZoneRepository()
        {
        }
        public DeliveryZoneRepository(DbContext db)
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

        public IEnumerable<DeliveryZone> Get(DeliveryZoneFilter filter)
        {
            try
            {
                var query = Db.Set<DeliveryZone>().AsQueryable();
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
    }
}
