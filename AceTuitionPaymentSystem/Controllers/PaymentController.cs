using AceTuitionPaymentSystem.Models;
using AceTuitionPaymentSystem.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AceTuitionPaymentSystem.Controllers
{
    public class PaymentController : Controller
    {
        private AceTuitionEntities objAceTuitionEntities = new AceTuitionEntities();

        // GET: Payment
        public ActionResult Index()
        {
            IEnumerable<PaymentViewModel> acceptedPaymentList = (from objPayment in objAceTuitionEntities.tb_payment
                                       where( objPayment.payment_status != "outstanding" && objPayment.payment_status != "pending")
                                       join objChild in objAceTuitionEntities.tb_child on objPayment.child_ic equals objChild.child_ic
                                       join objReceipt in objAceTuitionEntities.tb_receipt on objPayment.payment_id equals objReceipt.payment_id
                                       join objAdmin in objAceTuitionEntities.tb_admin on objPayment.admin_ic equals objAdmin.admin_ic
                                       join objParent in objAceTuitionEntities.tb_parent on objPayment.parent_ic equals objParent.parent_ic
                                       let fileDetails = from objFile in objAceTuitionEntities.tb_payment_file
                                       where (objPayment.payment_id == objFile.payment_id)
                                      
                                       select new PaymentFileViewModel()
                                                         {
                                                             payment_id = objFile.payment_id,
                                                             payment_file = objFile.payment_file,
                                                             payment_file_id = objFile.payment_file_id,
                                                         }
                                        orderby objPayment.payment_year descending,objPayment.payment_month descending, objPayment.payment_id descending
                                       select new PaymentViewModel()
                                       {
                                           payment_id = objPayment.payment_id,
                                           payment_date = objPayment.payment_date,
                                           payment_upload_date = objPayment.payment_upload_date,
                                           payment_status = objPayment.payment_status,
                                           payment_amount = objPayment.payment_amount,
                                           payment_month = objPayment.payment_month,
                                           payment_year = objPayment.payment_year,
                                           payment_outstanding = objPayment.payment_outstanding,
                                           receipt_outstanding=objReceipt.receipt_outstanding,
                                           payment_decision_date = (DateTime?)objPayment.payment_decision_date ?? DateTime.Now,
                                           parent_ic = objPayment.parent_ic,
                                           admin_ic = objPayment.admin_ic,
                                           child_ic = objPayment.child_ic,
                                           child_name_eng = objChild.child_name_eng,
                                           child_name_chinese = objChild.child_name_chinese,
                                           child_code = objChild.child_code,
                                           admin_name = objAdmin.admin_name,
                                           parent_name = objParent.parent_name,
                                           parent_phone = objParent.parent_phone,
                                           receipt_code=objReceipt.receipt_code,
                                           paymentFileList= fileDetails,

                                       }).ToList();

            int count = 0;
            var pending = objAceTuitionEntities.tb_payment.Where(x => x.payment_status == "pending").ToList();
            foreach (var p in pending)
            {
                count++;
            }
            TempData["count"] = count;
            return View(acceptedPaymentList);
        }
        public ActionResult Details(string ic, int payment_id)
        {

            IEnumerable<ReceiptViewModel> ReceiptDetails = (from objReceipt in objAceTuitionEntities.tb_receipt
                                                            join objPayment in objAceTuitionEntities.tb_payment on objReceipt.payment_id equals objPayment.payment_id
                                                            join objEarly in objAceTuitionEntities.tb_early_payment on objReceipt.early_id equals objEarly.early_id
                                                            join objChild in objAceTuitionEntities.tb_child on objReceipt.child_ic equals objChild.child_ic
                                                            join objParent in objAceTuitionEntities.tb_parent on objPayment.parent_ic equals objParent.parent_ic
                                                            
                                                            where (objReceipt.receipt_status == "accepted" && objReceipt.payment_id == payment_id && objReceipt.child_ic == ic)
                                                            let receiptPackage = from objPackage in objAceTuitionEntities.tb_receipt_package
                                                                                 where (objPackage.receipt_id == objReceipt.receipt_id)
                                                                                 select new ChildPackageViewModel()
                                                                                 {
                                                                                     value = objPackage.value,
                                                                                     package_id = objPackage.package_id,
                                                                                     package_english_name = objPackage.tb_package.package_english_name,
                                                                                     package_chinese_name = objPackage.tb_package.package_chinese_name,
                                                                                     package_price = objPackage.tb_package.package_price,


                                                                                 }
                                                            let receiptDiscount = from objDiscount in objAceTuitionEntities.tb_receipt_discount
                                                                                  where (objDiscount.receipt_id == objReceipt.receipt_id)
                                                                                  select new DiscountViewModel()
                                                                                  {
                                                                                      value = objDiscount.value,
                                                                                      discount_id = objDiscount.discount_id,
                                                                                      discount_english_name = objDiscount.tb_discount.discount_english_name,
                                                                                      discount_chinese_name = objDiscount.tb_discount.discount_chinese_name,
                                                                                      discount_recursive = objDiscount.tb_discount.discount_recursive,
                                                                                      discount_status = objDiscount.tb_discount.discount_status,
                                                                                      discount_value = objDiscount.tb_discount.discount_value,
                                                                                  }
                                                            let receiptAddon = from objAddon in objAceTuitionEntities.tb_receipt_addon
                                                                               where (objAddon.receipt_id == objReceipt.receipt_id)
                                                                               select new AddOnViewModel()
                                                                               {
                                                                                   value = objAddon.value,
                                                                                   addon_id = objAddon.addon_id,
                                                                                   addon_english_name = objAddon.tb_addon.addon_english_name,
                                                                                   addon_chinese_name = objAddon.tb_addon.addon_chinese_name,
                                                                                   addon_recursive = objAddon.tb_addon.addon_recursive,
                                                                                   addon_status = objAddon.tb_addon.addon_status,
                                                                                   addon_value = objAddon.tb_addon.addon_value,
                                                                               }
                                                            let childSubject = from objSubject in objAceTuitionEntities.tb_child_subject
                                                                               where (objSubject.child_ic == objReceipt.child_ic)
                                                                               select new ChildSubjectViewModel()
                                                                               {
                                                                                   child_ic = objSubject.child_ic,
                                                                                   subject_id = objSubject.subject_id,
                                                                                   subject_chinese_name = objSubject.tb_subject.subject_chinese_name,
                                                                                   subject_english_name = objSubject.tb_subject.subject_english_name,
                                                                                   subject_grade = objSubject.tb_subject.subject_grade,
                                                                                   subject_status = objSubject.tb_subject.subject_status

                                                                               }
                                                            let Early = from objEarly in objAceTuitionEntities.tb_early_payment
                                                                               where (objEarly.early_id!=6 && objEarly.early_id != 5)
                                                                               select new EarlypaymentViewModel()
                                                                               {
                                                                                   early_id=objEarly.early_id,
                                                                                   early_chinese_name=objEarly.early_chinese_name,
                                                                                   early_english_name=objEarly.early_english_name,
                                                                                   early_value=objEarly.early_value,

                                                                               }
                                                            select new ReceiptViewModel()
                                                            {
                                                                receipt_id = objReceipt.receipt_id,
                                                                payment_id = objReceipt.payment_id,
                                                                receipt_outstanding = objReceipt.receipt_outstanding,
                                                                receipt_status = objReceipt.receipt_status,
                                                                child_ic = objReceipt.child_ic,
                                                                early_id = objReceipt.early_id,
                                                                early_english_name = objEarly.early_english_name,
                                                                early_chinese_name = objEarly.early_chinese_name,
                                                                early_day = objEarly.early_day,
                                                                early_operation = objEarly.early_operation,
                                                                early_status = objEarly.early_status,
                                                                early_value = objEarly.early_value,
                                                                child_code = objChild.child_code,
                                                                child_name_chinese = objChild.child_name_chinese,
                                                                child_name_eng = objChild.child_name_eng,
                                                                child_num_subject = objChild.child_num_subject,
                                                                child_trans_day = objChild.child_trans_day,
                                                                receipt_addonList = receiptAddon,
                                                                receipt_packageList = receiptPackage,
                                                                receipt_discountList = receiptDiscount,
                                                                child_childsubject_list = childSubject,
                                                                parent_name = objParent.parent_name,
                                                                payment_amount = objPayment.payment_amount,
                                                                payment_date = objPayment.payment_date,
                                                                payment_month = objPayment.payment_month,
                                                                payment_outstanding = objPayment.payment_outstanding,
                                                                payment_balance_amount=(decimal)objPayment.payment_balance_amount,
                                                                receipt_code=objReceipt.receipt_code,
                                                                early_List = Early,

                                                            }).ToList();

            return PartialView(ReceiptDetails);
        }
    }
}