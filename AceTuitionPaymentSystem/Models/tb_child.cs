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
    
    public partial class tb_child
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public tb_child()
        {
            this.tb_addon = new HashSet<tb_addon>();
            this.tb_child_package = new HashSet<tb_child_package>();
            this.tb_child_subject = new HashSet<tb_child_subject>();
            this.tb_discount = new HashSet<tb_discount>();
            this.tb_receipt = new HashSet<tb_receipt>();
        }
    
        public string child_ic { get; set; }
        public string child_parent_ic { get; set; }
        public string child_name_eng { get; set; }
        public string child_name_chinese { get; set; }
        public string child_code { get; set; }
        public string child_school { get; set; }
        public string child_year { get; set; }
        public System.DateTime child_reg_date { get; set; }
        public int child_trans_day { get; set; }
        public int child_num_subject { get; set; }
        public string child_status { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tb_addon> tb_addon { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tb_child_package> tb_child_package { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tb_child_subject> tb_child_subject { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tb_discount> tb_discount { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tb_receipt> tb_receipt { get; set; }
    }
}
