using System.Collections.Generic;
using odec.Entity.DAL.Interop;

namespace odec.Processing.DAL.Interop
{
    public interface IOrderDetailsRepository<TKey, TContext, TOrderDetails, TOrderDetailsFilter> : IEntityOperations<TKey, TOrderDetails>, IContextRepository<TContext>
        where TKey : struct
    {
        IEnumerable<TOrderDetails> Get(TOrderDetailsFilter filter);
        IEnumerable<TOrderDetails> GetByOrder(TKey orderId);
    }
}