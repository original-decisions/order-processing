using odec.Framework.Generic;
using odec.Framework.Generic.Utility;

namespace odec.Server.Model.OrderProcessing.Filters
{
    public class OrderDetailsFilter<TKey>: FilterBase
    {
        public OrderDetailsFilter()
        {
            Sidx = "Id";
            Sord = "desc";
        }

        public TKey OrderId { get; set; }
        public Interval<decimal?> TotalInterval { get; set; }
    }
}
