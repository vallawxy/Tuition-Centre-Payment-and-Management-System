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
    
    public partial class tb_package
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public tb_package()
        {
            this.tb_child_package = new HashSet<tb_child_package>();
            this.tb_receipt_package = new HashSet<tb_receipt_package>();
        }
    
        public int package_id { get; set; }
        public string package_chinese_name { get; set; }
        public string package_english_name { get; set; }
        public decimal package_price { get; set; }
        public string package_operation { get; set; }
        public string package_status { get; set; }
        public string package_attribute { get; set; }
        public Nullable<int> package_subject_amount { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tb_child_package> tb_child_package { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tb_receipt_package> tb_receipt_package { get; set; }
    }
}
