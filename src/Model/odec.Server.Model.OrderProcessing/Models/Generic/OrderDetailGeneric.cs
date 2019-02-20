using odec.Framework.Generic;

namespace odec.Server.Model.OrderProcessing.Generic
{
    public class OrderDetailGeneric<T,TGoodId, TGood, TOrderId, TOrder> :Glossary<T>
    {
        public TGoodId GoodId { get; set; }

        public TGood Good { get; set; }

        public TOrderId OrderId { get; set; }
        
        public TOrder Order { get; set; }
        
        public int Quantity { get; set; }

        public decimal SubTotal { get; set; }
        
    }
}