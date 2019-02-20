using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace odec.Server.Model.OrderProcessing
{
    public class OrderOrderType
    {
    //    [Key, Column(Order = 0)]
        public int OrderId { get; set; }

        public Order Order { get; set; }

     //   [Key, Column(Order = 1)]
        public int OrderTypeId { get; set; }

        public OrderType OrderType { get; set; }


    }
}
