using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace odec.Server.Model.OrderProcessing.Money
{
    /// <summary>
    /// Table - AccountOperationsHistory
    /// </summary>
    public class AccountOperation
    {
      //  [Key,Column(Order = 1)]
        public DateTime OperationDate { get; set; }
     //   [Key, Column(Order = 0)]
        public int UserId { get; set; }
        public User.User User { get; set; }
        public decimal MoneyTransfered { get; set; }
        public int OperationTypeId { get; set; }
        public OperationType OperationType { get; set; }
    }
}