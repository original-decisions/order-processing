using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using odec.Framework.Generic;

namespace odec.Server.Model.OrderProcessing.Generic.UnifiedKey
{
    public class DeliveryMethodGeneric<TKey> :Glossary<TKey>
    {
        public DeliveryMethodGeneric()
        {
            NeedContacts = false;
        }

        [StringLength(50)]
        public override string Name { get; set; }
        [Required]
        [StringLength(1000)]
        [DefaultValue("")]
        public string Description { get; set; }

        [Required]
        [DefaultValue(false)]
        public bool NeedContacts { get; set; }

       

        //public virtual ICollection<TOrder> Orders { get; set; }
      //  public virtual ICollection<OrderHead> OrderHeads { get; set; }
    }
}
