using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using odec.Framework.Generic;


namespace odec.Server.Model.OrderProcessing.Generic.UnifiedKey
{
    /// <summary>
    /// Обобщенный класс - Заказ
    /// </summary>
    /// <typeparam name="TKey">Тип идентификатора</typeparam>
    /// <typeparam name="TContact">Тип контакта</typeparam>
    /// <typeparam name="TDeliveryMethod">Тип способа доставки</typeparam>
    /// <typeparam name="TPaymentMethod">Тип способа оплаты</typeparam>
    /// <typeparam name="TOrderState">Тип статуса заказа</typeparam>
    public class OrderGeneric<TKey, TContact, TDeliveryMethod, TPaymentMethod, TOrderState> : Glossary<TKey>
    {
        /// <summary>
        /// Имя
        /// </summary>
        [StringLength(128)]
        [Column("OrderNumber")]
        public override string Name { get; set; }
        /// <summary>
        /// Пользователь
        /// </summary>
        public TContact Contact { get; set; }
        /// <summary>
        /// Идентификатор пользователя
        /// </summary>
        [Required]
        public TKey ContactId { get; set; }
        /// <summary>
        /// Комментарий
        /// </summary>
        [Required(AllowEmptyStrings = true)]
        [StringLength(1500)]
        [DefaultValue("")]
        public string Comment { get; set; }
        /// <summary>
        /// Дата заказа
        /// </summary>
        [Required]
        public DateTime OrderDate { get; set; }
        /// <summary>
        /// полная стоимость
        /// </summary>
        [Required]
        [DataType("money")]
        public decimal Total { get; set; }
        /// <summary>
        /// Идентификатор типа доставки
        /// </summary>
        [Required]
        public TKey DeliveryMethodId { get; set; }
        /// <summary>
        /// ТИп доставки
        /// </summary>
        public TDeliveryMethod DeliveryMethod { get; set; }
        /// <summary>
        /// Идентификатор способа оплаты
        /// </summary>
        [Required]
        public TKey PaymentMethodId { get; set; }
        /// <summary>
        /// Способ оплаты
        /// </summary>
        public TPaymentMethod PaymentMethod { get; set; }
        /// <summary>
        /// Дата доставки
        /// </summary>
        public DateTime? DateDelivery { get; set; }
        /// <summary>
        /// TODO: think about charges and zones table
        /// </summary>
        public decimal DeliveryCost { get; set; }
        /// <summary>
        /// Идентификатор статуса заказа
        /// </summary>
        [Required]
        public TKey OrderStateId { get; set; }
        /// <summary>
        /// Статус заказа
        /// </summary>
        public TOrderState OrderState { get; set; }
    }
}