using System.ComponentModel.DataAnnotations;
using odec.Framework.Generic;

namespace odec.Server.Model.OrderProcessing
{
    public class OrderState : Glossary<int>
    {
        [StringLength(30)]
        public override string Name { get; set; }
    }
}
