using System.Collections.Generic;
using odec.Entity.DAL.Interop;

namespace odec.Processing.DAL.Interop
{
    public interface IOrderRepository<TKey, TContext, TOrder, TOrderDetail, TState, TOrderFilter, TOrderDetailsFilter> : IEntityOperations<TKey, TOrder>, IContextRepository<TContext>
        where TKey : struct
    {
        IEnumerable<TOrder> Get(TOrderFilter filter);
        void AddOrderDetails(IEnumerable<TOrderDetail> orderDetails,TOrder order);
        IEnumerable<TOrderDetail> GetOrderDetails(TOrderDetailsFilter filter);
        IEnumerable<TOrderDetail> GetOrderDetails(TOrder orderId);
        void ChangeState(TKey orderId, TState state);
    }
}