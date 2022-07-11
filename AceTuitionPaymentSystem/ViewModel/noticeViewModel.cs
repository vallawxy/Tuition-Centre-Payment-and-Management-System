using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AceTuitionPaymentSystem.ViewModel
{
    public class noticeViewModel
    {
        public int notice_id { get; set; }
        public string description { get; set; }
        public System.DateTime notice_date { get; set; }
        public string title { get; set; }
    }
}