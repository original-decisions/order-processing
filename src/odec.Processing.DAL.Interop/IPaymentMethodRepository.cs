using System.Collections.Generic;
using odec.Entity.DAL.Interop;

namespace odec.Processing.DAL.Interop
{
    public interface IPaymentMethodRepository<TKey, TContext, TPaymentMethod, TPaymentMethodFilter> : IEntityOperations<TKey, TPaymentMethod>, IContextRepository<TContext>
        where TKey : struct
    {
        IEnumerable<TPaymentMethod> Get(TPaymentMethodFilter filter);
    }
}