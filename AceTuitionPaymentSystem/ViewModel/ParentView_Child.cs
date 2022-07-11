using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AceTuitionPaymentSystem.Models;

namespace AceTuitionPaymentSystem.ViewModel
{
    public class ParentView_Child
    {
        public IEnumerable<ChildSubjectViewModel> child_childsubject_list;
        public IEnumerable<ChildPackageViewModel> child_childpackage_list;

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

        public string child_package_taken { get; set; }
        public string child_subject_taken { get; set; }


        public int package_id { get; set; }
        public string package_chinese_name { get; set; }
        public string package_english_name { get; set; }
        public decimal package_price { get; set; }
        public string package_operation { get; set; }
        public string package_status { get; set; }

        public int subject_id { get; set; }
        public string subject_chinese_name { get; set; }
        public string subject_english_name { get; set; }
        public string subject_grade { get; set; }
        public string subject_status { get; set; }


    }

}