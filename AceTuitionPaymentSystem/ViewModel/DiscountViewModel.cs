using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AceTuitionPaymentSystem.ViewModel
{
    public class DiscountViewModel
    {
        public int discount_id { get; set; }
        public string discount_english_name { get; set; }
        public string discount_chinese_name { get; set; }
        public string discount_recursive { get; set; }
        public decimal discount_value { get; set; }
        public string discount_status { get; set; }
        public string child_ic { get; set; }
        public decimal value { get; set; }
    }
}