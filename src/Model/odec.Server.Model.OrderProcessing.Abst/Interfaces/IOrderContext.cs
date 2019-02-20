using Microsoft.EntityFrameworkCore;

namespace odec.Server.Model.OrderProcessing.Abstractions.Interfaces
{
    public interface IOrderContext<TOrder, TOrderDetails, TDeliveryMethod, TPaymentMethod, TOrderState>
        where TOrder : class
        where TOrderDetails : class
        where TDeliveryMethod : class
        where TPaymentMethod : class
        where TOrderState : class
    {
        DbSet<TOrder> Orders { get; set; }
        DbSet<TOrderDetails> OrderDetails { get; set; }
        DbSet<TDeliveryMethod> DeliveryMethods { get; set; }
        DbSet<TPaymentMethod> PaymentMethods { get; set; }
        DbSet<TOrderState> OrderStates { get; set; }
    }
}
