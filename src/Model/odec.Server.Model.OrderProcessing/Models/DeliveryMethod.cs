using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using odec.Framework.Generic;
using odec.Server.Model.OrderProcessing.Generic.UnifiedKey;

namespace odec.Server.Model.OrderProcessing
{
    public class DeliveryMethod : Glossary<int>
    {
        [StringLength(50)]
        public override string Name { get; set; }
        [Required]
        [StringLength(1000)]
        [DefaultValue("")]
        public string Description { get; set; }
    }
}
