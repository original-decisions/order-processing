using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace odec.Server.Model.OrderProcessing.Money.Withdrawal
{
    public class WithdrawalApplication
    {
        [Key,DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public DateTime WithdrawalBefore { get; set; }

        [Required]
        public decimal Amount { get; set; }
        [Required]
        public DateTime DateCreated { get; set; }
        public int UserId { get; set; }
        public User.User User { get; set; }
        public int WithdrawalMethodId { get; set; }
        public WithdrawalMethod WithdrawalMethod { get; set; }

        [Required(AllowEmptyStrings = true)]
        public string Comment { get; set; }

        [Required]
        public bool IsApproved { get; set; }

        
    }
}
