using System.Collections.Generic;
using odec.Entity.DAL.Interop;

namespace odec.Processing.DAL.Interop
{
    public interface IDeliveryRepository<TKey, TContext, TDeliveryMethod, TDeliveryZone, TDeliveryMethodFilter> :
        IEntityOperations<TKey,TDeliveryMethod>,IContextRepository<TContext> 
        where TKey : struct
    {
        IEnumerable<TDeliveryMethod> Get(TDeliveryMethodFilter filter);
        IEnumerable<TDeliveryZone> GetDeliveryZones(TDeliveryMethod deliveryMethod);
        decimal GetDeliveryCharge(TDeliveryMethod deliveryMethod, TDeliveryZone zone);

    }
}