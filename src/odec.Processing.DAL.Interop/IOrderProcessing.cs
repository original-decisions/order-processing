using System.Collections.Generic;
using odec.Entity.DAL.Interop;

namespace odec.Processing.DAL.Interop
{
    public interface IOrderProcessing<TKey, TDbContext, TOrder,TState>:IContextRepository<TDbContext>
    {
        void ChangeOrderState(TOrder order, TState newState);
        void ChangeOrderState(TKey orderId, TState newState);
        IEnumerable<TState> GetAvailableStates(TOrder oder);
    }
}