using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AceTuitionPaymentSystem.ViewModel
{
    public class ReceiptViewModel
    {
        public IEnumerable<AddOnViewModel> receipt_addonList;
        public IEnumerable<ChildPackageViewModel> receipt_packageList; 
        public IEnumerable<DiscountViewModel> receipt_discountList;
        public IEnumerable<ChildSubjectViewModel> child_childsubject_list;
        public IEnumerable<EarlypaymentViewModel> early_List;
        public int receipt_id { get; set; }
        public int payment_id { get; set; }
        public string child_ic { get; set; }
        public decimal receipt_outstanding { get; set; }
        public string receipt_status { get; set; }
        public int early_id { get; set; }
        public int package_id { get; set; }
        public decimal value { get; set; }
        public int discount_id { get; set; }

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
        public string payment_id_list { get; set; }
        public string child_id_list { get; set; }
        public string child_payment_list { get; set; }

        public string parent_name { get; set; }
         public decimal payment_amount { get; set; }

        public DateTime payment_date { get; set; }

        public int payment_month { get; set; }
        public int payment_year { get; set; }
        public decimal payment_outstanding { get; set; }
        public decimal payment_balance_amount { get; set; }
        public string receipt_code { get; set; }
 
    }
}