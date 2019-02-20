using System;
using System.Collections.Generic;
using odec.Entity.DAL.Interop;

namespace odec.Processing.DAL.Interop
{
    public interface IWithdrawalProcessing<in TKey,TContext, TWithdrawalApplication, in TWithdrawalMethod, in TUser, in TWithdrawalApplicationFilter> : 
        IContextRepository<TContext>
    {
        TWithdrawalApplication GetById(TKey id);
        /// <summary>
        /// Creates the withdrawal application
        /// </summary>
        /// <param name="withdrawalMethodId"></param>
        /// <param name="userId"></param>
        /// <param name="withdrawalBefore"></param>
        /// <param name="comment"></param>
        /// <returns></returns>
        TWithdrawalApplication CreateApplication(TKey withdrawalMethodId, TKey userId, DateTime withdrawalBefore,decimal amount, string comment);
        /// <summary>
        /// Creates the withdrawal application
        /// </summary>
        /// <param name="withdrawalMethod"></param>
        /// <param name="user"></param>
        /// <param name="withdrawalBefore"></param>
        /// <param name="comment"></param>
        /// <returns></returns>
        TWithdrawalApplication CreateApplication(TWithdrawalMethod withdrawalMethod, TUser user, DateTime withdrawalBefore, decimal amount, string comment);
        /// <summary>
        /// Creates the withdrawal application
        /// </summary>
        /// <param name="application"></param>
        /// <returns></returns>
        TWithdrawalApplication CreateApplication(TWithdrawalApplication application);
        /// <summary>
        /// Gets the withdrawal applications specified by filter
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        IEnumerable<TWithdrawalApplication> GetApplications(TWithdrawalApplicationFilter filter);

        void ApproveWithdrawal(TWithdrawalApplication application);
    }
}