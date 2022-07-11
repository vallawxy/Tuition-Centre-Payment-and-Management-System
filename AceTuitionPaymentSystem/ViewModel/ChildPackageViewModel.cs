using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AceTuitionPaymentSystem.ViewModel
{
    public class ChildPackageViewModel
    {
        public string child_ic { get; set; }
        public int package_id { get; set; }
        public string package_english_name { get; set; }
        public decimal package_price { get; set; }
        public string package_operation { get; set; }
        public string package_chinese_name { get; set; }
        public string package_status { get; set; }
        public decimal value { get; set; }
    }
}