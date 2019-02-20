using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace odec.Server.Model.OrderProcessing
{
    public class OrderDetail
    {
        [Key,DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int OrderId { get; set; }
        public Order Order { get; set; }
        public string JsonEntityDetails { get; set; }
        public int EntityCount { get; set; }
        public decimal DiscountedCost { get; set; }
        public decimal MomentCost { get; set; }
        public decimal Total { get; set; }
    }
}
