using System.ComponentModel.DataAnnotations;
using odec.Framework.Generic;

namespace odec.Server.Model.OrderProcessing.Money.Withdrawal
{
    public class WithdrawalMethod:Glossary<int>
    {
        [Required(AllowEmptyStrings = true)]
        public string Description { get; set; }
    }
}