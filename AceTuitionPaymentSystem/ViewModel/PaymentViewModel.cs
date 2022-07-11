using AceTuitionPaymentSystem.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AceTuitionPaymentSystem.ViewModel
{
    public class PaymentViewModel
    {
        public IEnumerable<OutstsandingViewModel> paymentList { get; set; }
        public IEnumerable<PaymentFileViewModel> paymentFileList { get; set; }

        public IEnumerable<AddOnViewModel> receipt_addonList;
        public IEnumerable<ChildPackageViewModel> receipt_packageList;
        public IEnumerable<DiscountViewModel> receipt_discountList;
        public IEnumerable<ChildSubjectViewModel> child_childsubject_list;
        public IEnumerable<EarlypaymentViewModel> early_List;

        public int payment_id { get; set; }
        [DisplayName("Payment Date")]
        [Required]
        public System.DateTime payment_date { get; set; }
        
        public System.DateTime payment_upload_date { get; set; }

        public System.DateTime payment_decision_date { get; set; }

        public string payment_status { get; set; }
        [DisplayName("Payment Amount")]
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Amount must be greater than 0!")] //validate
        public decimal payment_amount { get; set; }
        [DisplayName("Receipt")]
        [Required]
        public string payment_file { get; set; }

        [DisplayName("Payment Proof")]
        public HttpPostedFileBase[] receipt { get; set; }
        //public string payment_list { get; set; }
       
        public int payment_month { get; set; }
        public int payment_year { get; set; }
        public decimal payment_outstanding { get; set; }
        public string parent_ic { get; set; }
        public string admin_ic { get; set; }
        public string child_ic { get; set; }
        public string child_name_eng { get; set; }
        public string child_name_chinese { get; set; }
        public string child_code { get; set; }
        public string admin_name { get; set; }
        public string parent_name { get; set; }
        public string parent_phone { get; set; }

        public string notice_list { get; set; }
        public string payment_id_list { get; set; }
       
        public string child_id_list { get; set; }
        public int early_id { get; set; }
        public string early_chinese_name { get; set; }
        public string early_english_name { get; set; }
        public decimal early_value { get; set; }
        public int early_day { get; set; }
        public string early_status { get; set; }
        public string receipt_status { get; set; }
        public int receipt_id { get; set; }
        public decimal receipt_outstanding { get; set; }
        public string early_operation { get; set; }
        public int child_num_subject { get; set; }
        public int child_trans_day { get; set; }
        public IEnumerable<PaymentFileViewModel> payment_file_list;
        public decimal payment_balance_amount { get; set; }

        public string receipt_code { get; set; }

    }
}

