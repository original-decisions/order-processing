using System.Collections.Generic;
using odec.Entity.DAL.Interop;

namespace odec.Processing.DAL.Interop
{
    public interface IDeliveryZoneRepository<in TKey, TContext, TDeliveryZone, in TDeliveryZoneFilter> : IEntityOperations<TKey, TDeliveryZone>, IContextRepository<TContext> where TKey : struct
    {
        IEnumerable<TDeliveryZone> Get(TDeliveryZoneFilter filter);
    }
}