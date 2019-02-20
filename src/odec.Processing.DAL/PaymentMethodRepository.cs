using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using odec.Entity.DAL;
using odec.Processing.DAL.Interop;
using odec.Server.Model.OrderProcessing;
using odec.Server.Model.OrderProcessing.Filters;
using odec.Framework.Extensions;
using odec.Framework.Logging;

namespace odec.Processing.DAL
{
    public class PaymentMethodRepository : OrmEntityOperationsRepository<int, PaymentMethod, DbContext>, IPaymentMethodRepository<int, DbContext, PaymentMethod, PaymentMethodFilter>
    {
        public IEnumerable<PaymentMethod> Get(PaymentMethodFilter filter)
        {
            try
            {
                var query = Db.Set<PaymentMethod>().AsQueryable();
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

        public PaymentMethodRepository() { }

        public PaymentMethodRepository(DbContext db)
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
