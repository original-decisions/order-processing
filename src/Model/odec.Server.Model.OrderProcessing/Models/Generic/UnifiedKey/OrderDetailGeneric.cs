using odec.Framework.Generic;

namespace odec.Server.Model.OrderProcessing.Generic.UnifiedKey
{
    //TODO:Refactor this class
    /// <summary>
    /// Обобщенный класс - детали заказа
    /// </summary>
    /// <typeparam name="TKey">Тип идентификатора</typeparam>
    /// <typeparam name="TGood">Тип товара</typeparam>
    /// <typeparam name="TOrder">Тип заказа</typeparam>
    public class OrderDetailGeneric<TKey, TGood, TOrder> : Glossary<TKey>
    {
        /// <summary>
        /// Идентификатор товара
        /// </summary>
        public TKey GoodId { get; set; }
        /// <summary>
        /// Товар
        /// </summary>
        public TGood Good { get; set; }
        /// <summary>
        /// Идентификатор заказа
        /// </summary>
        public TKey OrderId { get; set; }
        /// <summary>
        /// Заказ
        /// </summary>
        public TOrder Order { get; set; }
        /// <summary>
        /// кол-во
        /// </summary>
        public int Quantity { get; set; }
        /// <summary>
        /// Под стоимость 
        /// </summary>
        public decimal SubTotal { get; set; }
    }
}