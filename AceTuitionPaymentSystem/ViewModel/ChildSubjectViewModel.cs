using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AceTuitionPaymentSystem.ViewModel
{
    public class ChildSubjectViewModel
    {
        public int subject_id { get; set; }
        public string child_ic { get; set; }
        public string subject_chinese_name { get; set; }
        public string subject_english_name { get; set; }
        public string subject_grade { get; set; }
        public string subject_status { get; set; }
    }
}