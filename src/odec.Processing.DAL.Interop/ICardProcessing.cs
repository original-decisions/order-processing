using odec.Entity.DAL.Interop;

namespace odec.Processing.DAL.Interop
{
    public interface ICardProcessing<TKey, TContext, TCard,TUser> : IEntityOperations<TKey, TCard>, IContextRepository<TContext> 
        where TKey : struct
    {
        TCard GetUserCard(TUser user);
    }
}