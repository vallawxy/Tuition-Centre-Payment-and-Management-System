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
    
    public partial class tb_early_payment
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public tb_early_payment()
        {
            this.tb_receipt = new HashSet<tb_receipt>();
        }
    
        public int early_id { get; set; }
        public string early_english_name { get; set; }
        public string early_chinese_name { get; set; }
        public decimal early_value { get; set; }
        public int early_day { get; set; }
        public string early_status { get; set; }
        public string early_operation { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tb_receipt> tb_receipt { get; set; }
    }
}
