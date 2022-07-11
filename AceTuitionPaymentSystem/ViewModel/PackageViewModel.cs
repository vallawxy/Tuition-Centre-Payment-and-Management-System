using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AceTuitionPaymentSystem.ViewModel
{
    public class PackageViewModel
    {
        public int package_id { get; set; }
        public string package_english_name { get; set; }
        public decimal package_price { get; set; }
        public string package_operation { get; set; }
        public string package_chinese_name { get; set; }
        public string package_status { get; set; }
        public string package_attribute { get; set; }
        public Nullable<int> package_subject_amount { get; set; }

    }

}