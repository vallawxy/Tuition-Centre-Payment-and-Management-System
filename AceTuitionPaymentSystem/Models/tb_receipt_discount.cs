//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace AceTuitionPaymentSystem.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class tb_receipt_discount
    {
        public int receipt_id { get; set; }
        public int discount_id { get; set; }
        public decimal value { get; set; }
        public int receipt_discount_id { get; set; }
    
        public virtual tb_discount tb_discount { get; set; }
        public virtual tb_receipt tb_receipt { get; set; }
    }
}
