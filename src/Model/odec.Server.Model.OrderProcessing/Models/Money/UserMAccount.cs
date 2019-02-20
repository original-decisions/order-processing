using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace odec.Server.Model.OrderProcessing.Money
{
    public class UserMAccount
    {
        [Key]
        [ForeignKey("User")]
        public int UserId { get; set; }

        public User.User User { get; set; }
        public decimal CurrentMoney { get; set; }
        public decimal BlockedMoney { get; set; }
    }
}