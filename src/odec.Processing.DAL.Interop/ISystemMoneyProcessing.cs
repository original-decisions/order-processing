using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using odec.Entity.DAL.Interop;

namespace odec.Processing.DAL.Interop
{
    public interface ISystemMoneyProcessing<TKey, TDbContext, TUser, TUserBalance, TOperationType, TOrder, TAccountOperation, TPaymentMethod> : IContextRepository<TDbContext>
        where TOperationType : class

    {
        /// <summary>
        /// Gets user balance
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        TUserBalance GetUserBalance(TKey userId);
        /// <summary>
        /// Gets user balance
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        TUserBalance GetUserBalance(TUser user);

        /// <summary>
        /// Creates transfer order based on params
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="amount"></param>
        /// <param name="operationType"></param>
        /// <returns></returns>
        TOrder MakeTransferOrder(TKey userId, decimal amount, TOperationType operationType,TPaymentMethod paymentMethod);
        /// <summary>
        /// Gets a transfer orders history
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="operationType"></param>
        /// <returns></returns>
        IEnumerable<TAccountOperation> GetTransferHistory(TKey userId, TOperationType operationType = null);
        
        /// <summary>
        /// Blocks(reserves) a user funds
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="amount"></param>
        /// <returns></returns>
        TUserBalance BlockFunds(TKey userId, decimal amount);

        TUserBalance ReleaseBlockedMoney(TKey userId, decimal amount);
        Tuple<TUserBalance,TUserBalance> TransferMoney(TKey userId, decimal amount,TKey destinationUserId);
        Tuple<TUserBalance, TUserBalance> TransferBlockedMoney(TKey userId, decimal amount, TKey destinationUserId);

        
        /// <summary>
        /// Adds an amount of money to user funds
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="amount"></param>
        /// <returns></returns>
        TUserBalance AddFunds(TKey userId, decimal amount);
        /// <summary>
        /// Gets reserved(blocked) money
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        decimal GetReservedMoney(TKey userId);
        /// <summary>
        /// Gets reserved(blocked) money
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        decimal GetReservedMoney(TUser user);

        //todo: move to another repository
        

    }
}
