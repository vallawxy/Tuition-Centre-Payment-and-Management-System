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
    
    public partial class tb_balance
    {
        public int balance_id { get; set; }
        public string parent_ic { get; set; }
        public decimal balance_amount { get; set; }
        public int month { get; set; }
        public string status { get; set; }
    
        public virtual tb_parent tb_parent { get; set; }
    }
}
