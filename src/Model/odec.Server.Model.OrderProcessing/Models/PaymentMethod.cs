using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using odec.Framework.Generic;

namespace odec.Server.Model.OrderProcessing
{
    /// <summary>
    /// Server Model - Payment method
    /// </summary>
    public class PaymentMethod :Glossary<int>
    {
        /// <summary>
        /// Payment Method Name
        /// </summary>
        [StringLength(50)]
        public override string Name { get; set; }

        //public virtual ICollection<Order> Orders { get; set; }
    }
}
