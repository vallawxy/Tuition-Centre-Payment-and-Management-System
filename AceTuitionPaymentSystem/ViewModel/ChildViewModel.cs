using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AceTuitionPaymentSystem.ViewModel
{
    public class ChildViewModel
    {
        public string child_ic { get; set; }
        public string child_name_eng { get; set; }
        public string child_name_chinese { get; set; }
        public string child_parent_ic { get; set; }
        public string child_code { get; set; }
        public string child_school { get; set; }
        public string child_year { get; set; }
        public System.DateTime child_reg_date { get; set; }
        public int child_trans_day { get; set; }
        public int child_num_subject { get; set; }
        public string child_status { get; set; }
        public string child_subject_list { get; set; }
        public string child_package_list { get; set; }
        public string child_addon_english_name_list { get; set; }
        public string child_addon_chinese_name_list { get; set; }
        public string child_addon_value_list { get; set; }
        public string child_addon_recursive_list { get; set; }
        public string child_discount_english_name_list { get; set; }
        public string child_discount_chinese_name_list { get; set; }
        public string child_discount_value_list { get; set; }
        public string child_discount_recursive_list { get; set; }
        public string child_package_taken { get; set; }
        public string child_subject_taken { get; set; }
        public string child_addon_taken { get; set; }
        public string child_discount_taken { get; set; }
    }
}