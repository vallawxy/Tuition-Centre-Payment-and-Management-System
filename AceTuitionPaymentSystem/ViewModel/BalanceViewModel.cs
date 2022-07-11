using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AceTuitionPaymentSystem.ViewModel
{
    public class BalanceViewModel
    {
        public int balance_id { get; set; }
        public string parent_ic { get; set; }
        public decimal balance_amount { get; set; }
        public int month { get; set; }
        public string status { get; set; }
    }
}