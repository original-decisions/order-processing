using System;
using odec.Framework.Generic;
using odec.Framework.Generic.Utility;

namespace odec.Server.Model.OrderProcessing.Filters
{
    public class WithdrawalApplicationFilter<TUserKey, TKey>:FilterBase
    {
        public WithdrawalApplicationFilter()
        {
            Sidx = "Id";
            Sord = "desc";
        }

        public TUserKey UserId { get; set; }
        public TKey WithdrawalMethodId { get; set; }
        public Interval<DateTime?> DateCreatedInterval { get; set; }
        public Interval<decimal?> Amount { get; set; }
        public Interval<DateTime?> WithdrawalBeforeInterval { get; set; }
    }
}
