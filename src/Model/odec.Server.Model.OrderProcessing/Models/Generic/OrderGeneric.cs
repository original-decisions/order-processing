using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using odec.Framework.Generic;

namespace odec.Server.Model.OrderProcessing.Generic
{
    public class OrderGeneric<T, TUserId, TUser, TDeliveryMethodId, TDeliveryMethod, TPaymentMethodId, TPaymentMethod, TOrderStateId, TOrderState, TAdressId, TAddress> : Glossary<T>
    {

        [StringLength(30)]
        [Column("OrderNumber")]
        public override string Name { get; set; }

        public TUser User { get; set; }

        [Required]
        public TUserId UserId { get; set; }

        [Required(AllowEmptyStrings = true)]
        [StringLength(1500)]
        [DefaultValue("")]
        public string Comment { get; set; }

        [Required]
        public DateTime OrderDate { get; set; }

        //[Required]
        //public int Number { get; set; }


        [Required]
        [DataType("money")]
        public decimal Total { get; set; }

        [Required]
        public TDeliveryMethodId DeliveryMethodId { get; set; }


        public TDeliveryMethod DeliveryMethod { get; set; }

        [Required]
        public TPaymentMethodId PaymentMethodId { get; set; }

        public TPaymentMethod PaymentMethod { get; set; }

        //[DataType("date")]
        public DateTime DateDelivery { get; set; }

        [Required]
        public TOrderStateId OrderStateId { get; set; }

        public TOrderState OrderState { get; set; }

        [Column("DeliveryAdressID")]
        public TAdressId AdressId { get; set; }

        public TAddress Adress { get; set; }

        //[Column("ContactPhoneID")]
        //public int PhoneID { get; set; }
        //public Phone Phone { get; set; }




    }
}