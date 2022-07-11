using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AceTuitionPaymentSystem.ViewModel
{
    public class OutstsandingViewModel
    {
        public int payment_id { get; set; }
       
        [Required]
       
        public System.DateTime payment_date { get; set; }

        public System.DateTime payment_upload_date { get; set; }

        public string payment_status { get; set; }
    
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Amount must be greater than 0!")] //validate
        public decimal payment_amount { get; set; }

        public string payment_file { get; set; }
        public string payment_list { get; set; }

        public int payment_month { get; set; }
        public int payment_year { get; set; }
        public decimal payment_outstanding { get; set; }
        public string parent_ic { get; set; }
        public string parent_name { get; set; }
        public string admin_ic { get; set; }
        public string child_ic { get; set; }
        public string child_name_eng { get; set; }
        public string child_name_chinese { get; set; }
        public string child_code { get; set; }
    }
}