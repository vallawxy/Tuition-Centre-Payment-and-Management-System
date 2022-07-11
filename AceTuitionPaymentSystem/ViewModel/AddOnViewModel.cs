using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AceTuitionPaymentSystem.ViewModel
{
    public class AddOnViewModel
    {
        public int addon_id { get; set; }
        public string addon_english_name { get; set; }
        public string addon_chinese_name { get; set; }
        public string addon_recursive { get; set; }
        public decimal addon_value { get; set; }
        public string addon_status { get; set; }
        public string child_ic { get; set; }
        public decimal value { get; set; }
    }
}