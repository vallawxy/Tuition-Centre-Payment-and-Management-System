using AceTuitionPaymentSystem.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AceTuitionPaymentSystem.ViewModel
{
    public class Payment_FileViewModel
    {
        public IEnumerable<OutstsandingViewModel> paymentList { get; set; }
        public IEnumerable<PaymentFileViewModel> paymentFileList { get; set; }
        //public IEnumerable<OutstsandingViewModel> paymentList;
        //public IEnumerable<PaymentFileViewModel> paymentFileList;

        public IEnumerable<AddOnViewModel> receipt_addonList;
        public IEnumerable<ChildPackageViewModel> receipt_packageList;
        public IEnumerable<DiscountViewModel> receipt_discountList;
        public IEnumerable<ChildSubjectViewModel> child_childsubject_list;

        public int receipt_id { get; set; }

        [DisplayName("Payment Date")]
        [Required]
        public System.DateTime payment_date { get; set; }
        public int payment_id { get; set; }
        public string child_ic { get; set; }
        public string parent_ic { get; set; }
        public decimal receipt_outstanding { get; set; }
        public string receipt_status { get; set; }
        public int early_id { get; set; }
        public int package_id { get; set; }
        public decimal value { get; set; }
        public int discount_id { get; set; }

        public decimal payment_balance_amount { get; set; }
        public int addon_id { get; set; }
        public string child_name_eng { get; set; }
        public string child_name_chinese { get; set; }
        public string child_parent_ic { get; set; }
        public string child_code { get; set; }

        public int child_trans_day { get; set; }
        public int child_num_subject { get; set; }


        public string early_chinese_name { get; set; }
        public string early_english_name { get; set; }
        public decimal early_value { get; set; }
        public int early_day { get; set; }
        public string early_status { get; set; }
        public string early_operation { get; set; }

       

        public System.DateTime payment_upload_date { get; set; }



        public int payment_month { get; set; }
        public int payment_year { get; set; }
        public string payment_status { get; set; }

        public string payment_file { get; set; }
        public decimal payment_outstanding { get; set; }
        public string parent_name { get; set; }

        public decimal payment_amount { get; set; }

        

        public IEnumerable<PaymentFileViewModel> payment_file_list;











    }
}