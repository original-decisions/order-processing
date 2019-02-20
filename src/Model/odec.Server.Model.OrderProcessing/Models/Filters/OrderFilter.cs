using System;
using odec.Framework.Generic;
using odec.Framework.Generic.Utility;

namespace odec.Server.Model.OrderProcessing.Filters
{
    public class OrderFilter<TKey> : FilterBase
    {
        public OrderFilter()
        {
            Sord = "desc";
            Sidx = "DateCreated";
        }

        public TKey UserId { get; set; }
        public Interval<DateTime?> DateCreatedInterval { get; set; }

        public Interval<DateTime?> DateDeliveredInterval { get; set; }
        public Interval<decimal?> TotalInterval { get; set; }
        public string Code { get; set; }
        public TKey OrderStateId { get; set; }
        public TKey OrderTypeId { get; set; }
        public TKey DeliveryMethodId { get; set; }
        public TKey PaymentMethodId { get; set; }
    }
}
