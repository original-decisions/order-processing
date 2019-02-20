using odec.Server.Model.OrderProcessing.Generic.UnifiedKey;

namespace odec.Server.Model.OrderProcessing
{
    /// <summary>
    /// Order Server Model
    /// </summary>
    public class Order : OrderGeneric<int, Contact.Contact, DeliveryMethod, PaymentMethod,OrderState>
    {
        /// <summary>
        /// Delivery Zone Identity
        /// </summary>
        public int? DeliveryZoneId { get; set; }
        /// <summary>
        /// Delivery Zone Navigation property
        /// </summary>
        public DeliveryZone DeliveryZone { get; set; }

    }
}