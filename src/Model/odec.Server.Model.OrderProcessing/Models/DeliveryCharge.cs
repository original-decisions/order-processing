using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace odec.Server.Model.OrderProcessing
{
    public class DeliveryCharge
    {
        [Required,DefaultValue(0)]
        public decimal ChargeValue { get; set; }
      //  [Key,Column(Order = 0)]
        public int ZoneId { get; set; }
        public DeliveryZone Zone { get; set; }
      //  [Key, Column(Order = 1)]
        public int DeliveryMethodId { get; set; }
        public DeliveryMethod DeliveryMethod { get; set; }
    }
}