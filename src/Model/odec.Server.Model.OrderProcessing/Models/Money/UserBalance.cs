namespace odec.Server.Model.OrderProcessing.Money
{
    public class UserBalance
    {
        public int UserId { get; set; }
        public User.User User { get; set; }
        public decimal CurrentMoney { get; set; }
        public decimal BlockedMoney { get; set; }


    }
}
