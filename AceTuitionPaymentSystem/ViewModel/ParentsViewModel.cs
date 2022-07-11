using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AceTuitionPaymentSystem.ViewModel
{
    public class ParentsViewModel
    {
        public string parent_ic { get; set; }
        public string parent_password { get; set; }
        public string parent_name { get; set; }
        public string parent_address { get; set; }
        public string parent_phone { get; set; }
        public string parent_status { get; set; }
        public string LoginErrorMessage { get; set; }
        public string parent_edit_id { get; set; }
    }
}