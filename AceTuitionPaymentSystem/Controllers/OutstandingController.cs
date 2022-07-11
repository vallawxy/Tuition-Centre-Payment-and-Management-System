using AceTuitionPaymentSystem.Models;
using AceTuitionPaymentSystem.ViewModel;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace AceTuitionPaymentSystem.Controllers
{
    public class OutstandingController : Controller
    {
        // GET: Outstanding
        private AceTuitionEntities objAceTuitionEntities;
        public OutstandingController()
        {
            objAceTuitionEntities = new AceTuitionEntities();
        }

        // GET: Child
        public ActionResult Index()
        {

            IEnumerable<OutstsandingViewModel> listOfPaymentViewModel = (from objPayment in objAceTuitionEntities.tb_payment
                                                                    join objChild in objAceTuitionEntities.tb_child on objPayment.child_ic equals objChild.child_ic
                                                                    join objParent in objAceTuitionEntities.tb_parent on objPayment.parent_ic equals objParent.parent_ic
                                                                    where (objPayment.payment_status=="outstanding")
                                                                    orderby objPayment.payment_year,objPayment.payment_month, objChild.child_code
                                                                    select new OutstsandingViewModel()
                                                                {
                                                                        payment_id=objPayment.payment_id,
                                                                        payment_date = objPayment.payment_date,
                                                                        payment_month = objPayment.payment_month,
                                                                        payment_year = objPayment.payment_year,
                                                                        payment_status = objPayment.payment_status,
                                                                        payment_upload_date = objPayment.payment_upload_date,
                                                                        parent_ic = objPayment.parent_ic,
                                                                        payment_outstanding = objPayment.payment_outstanding,
                                                                        child_ic = objChild.child_ic,
                                                                        child_name_eng = objChild.child_name_eng,
                                                                        child_name_chinese = objChild.child_name_chinese,
                                                                        child_code = objChild.child_code,
                                                                        parent_name=objParent.parent_name,

                                                                    }).ToList();

            IEnumerable<PaymentFileViewModel> listOfFileViewModel = (from objPayment in objAceTuitionEntities.tb_payment
                                                                 join objChild in objAceTuitionEntities.tb_child on objPayment.child_ic equals objChild.child_ic
                                                                 join objParent in objAceTuitionEntities.tb_parent on objPayment.parent_ic equals objParent.parent_ic
                                                                     //join objFile in objAceTuitionEntities.tb_payment_file on objPayment.payment_id equals objFile.payment_id
                                                                     //where (objPayment.payment_status == "pending" || objPayment.payment_status == "rejected")
                                                                 where (objPayment.payment_status == "pending")
                                                                 orderby objPayment.payment_year, objPayment.payment_month, objChild.child_code
                                                                 select new PaymentFileViewModel()
                                                                    {
                                                                        payment_id = objPayment.payment_id,
                                                                        payment_date = objPayment.payment_date,
                                                                        payment_month = objPayment.payment_month,
                                                                        payment_year = objPayment.payment_year,
                                                                        payment_status = objPayment.payment_status,
                                                                        payment_upload_date = objPayment.payment_upload_date,
                                                                        //payment_file = objFile.payment_file,
                                                                        parent_ic = objPayment.parent_ic,
                                                                        payment_outstanding = objPayment.payment_outstanding,
                                                                        payment_amount = objPayment.payment_amount,
                                                                        child_ic = objChild.child_ic,
                                                                        child_name_eng = objChild.child_name_eng,
                                                                        child_name_chinese = objChild.child_name_chinese,
                                                                        child_code = objChild.child_code,
                                                                     parent_name = objParent.parent_name,

                                                                 }).ToList();

            int count = 0;
            var pending = objAceTuitionEntities.tb_payment.Where(x => x.payment_status == "pending").ToList();
            foreach (var p in pending)
            {
                count++;
            }
            TempData["count"] = count;

            var mixmodel = new Payment_FileViewModel();

            mixmodel.paymentList = listOfPaymentViewModel;
            mixmodel.paymentFileList = listOfFileViewModel;
           

            return View(mixmodel);
        }
        public ActionResult AddorEdit(int? id)
        {

            var payment = objAceTuitionEntities.tb_payment.Find(id) ?? new tb_payment();
            return PartialView("AddorEdit", payment);
        }

        public ActionResult byCash(string child, string parent, int payment)
        {
            

            var today = DateTime.Now.Day;
            var month = DateTime.Now.Month;
            int Dday = Convert.ToInt32(today);

            using (var context = new AceTuitionEntities())
            {
                var PaymentList = context.tb_payment.Where(x => x.payment_id == payment && x.payment_status == "outstanding").SingleOrDefault();
                
                if (PaymentList == null)
                {
                    
                    return RedirectToAction("Index", "Outstanding");
                }

                var receipt = context.tb_receipt.Where(x => x.payment_id == PaymentList.payment_id).SingleOrDefault();
                receipt.early_id = 6;

                var recPackage = context.tb_receipt_package.Where(x => x.receipt_id == receipt.receipt_id).ToList();

                var childcareEarly = false;
                var tuitionEarly = false;
                var tutionEarlyChecked = false;
                var childcareEarlyChecked = false;

                foreach (var rPackage in recPackage)
                {
                    var package = context.tb_package.Where(x => x.package_id == rPackage.package_id).SingleOrDefault();
                    if (package.package_operation == "Childcare")
                    {
                        childcareEarly = true;
                    }

                    if (package.package_operation == "Tuition")
                    {
                        tuitionEarly = true;
                    }
                }




                if (childcareEarly)
                {
                    var earlyPackage = context.tb_early_payment.Where(x => x.early_id == 3 && x.early_status == "active").SingleOrDefault();
                    if (earlyPackage != null)
                    {
                        if (earlyPackage.early_day >= Dday)
                        {
                            receipt.early_id = earlyPackage.early_id;
                            childcareEarlyChecked = true;
                        }

                    }
                }


                if (tuitionEarly)
                {
                    var earlyPackage = context.tb_early_payment.Where(x => x.early_id == 1 && x.early_status == "active").SingleOrDefault();
                    if (earlyPackage != null)
                    {
                        if (earlyPackage.early_day >= Dday)
                        {
                            receipt.early_id = earlyPackage.early_id;
                            tutionEarlyChecked = true;

                            receipt.early_id = 1;
                        }

                    }
                }

                if (tutionEarlyChecked && childcareEarlyChecked)
                {
                    receipt.early_id = 5;
                }

                context.SaveChanges();

            }


            IEnumerable<PaymentViewModel> Details = (from objReceipt in objAceTuitionEntities.tb_receipt
                                                     where (objReceipt.payment_id == payment)
                                                     join objPayment in objAceTuitionEntities.tb_payment on objReceipt.payment_id equals objPayment.payment_id
                                                     join objEarly in objAceTuitionEntities.tb_early_payment on objReceipt.early_id equals objEarly.early_id
                                                     join objChild in objAceTuitionEntities.tb_child on objReceipt.child_ic equals objChild.child_ic
                                                     join objParent in objAceTuitionEntities.tb_parent on objChild.child_parent_ic equals objParent.parent_ic
                                                     where (objParent.parent_ic == parent)

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
                                                     select new PaymentViewModel()
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
                                                         payment_date = objPayment.payment_date,
                                                         payment_amount = objPayment.payment_amount,
                                                         parent_ic = objParent.parent_ic,
                                                         parent_name = objParent.parent_name

                                                     }).ToList();


            IEnumerable<EarlypaymentViewModel> EarlyPayment = (from objEarly in objAceTuitionEntities.tb_early_payment
                                                               where(objEarly.early_id==1 || objEarly.early_id == 3)
                                                               select new EarlypaymentViewModel()
                                                               {
                                                                   early_id = objEarly.early_id,
                                                                    early_chinese_name = objEarly.early_chinese_name,
                                                                    early_english_name = objEarly.early_english_name,
                                                                    early_value = objEarly.early_value,
                                                                    early_day = objEarly.early_day,
                                                                    early_status = objEarly.early_status,
                                                                    early_operation = objEarly.early_operation
                                                               }).ToList();

            IEnumerable<BalanceViewModel> AccBalance = (from objBalance in objAceTuitionEntities.tb_balance
                                                             join objParent in objAceTuitionEntities.tb_parent on objBalance.parent_ic equals objParent.parent_ic
                                                             where (objBalance.status =="active" && objBalance.parent_ic == parent)
                                                               select new BalanceViewModel()
                                                               {
                                                                   parent_ic = objBalance.parent_ic,
                                                                   balance_id = objBalance.balance_id,
                                                                   balance_amount = objBalance.balance_amount,
                                                                   month = objBalance.month,
                                                                   status = objBalance.status,

                                                               }).ToList();

            int count = 0;
            var pending = objAceTuitionEntities.tb_payment.Where(x => x.payment_status == "pending").ToList();
            foreach (var p in pending)
            {
                count++;
            }
            TempData["count"] = count;

            var mixmodel = new ParentMake_PaymentViewModel();

            mixmodel.paymentList = Details;
            mixmodel.earlyList = EarlyPayment;
            mixmodel.accBalance = AccBalance;


            return View(mixmodel);
        }

        public ActionResult CreateOrUpdate(tb_payment payment)
        {

            return Json(data: new { Success = true, Message = "Payment Done." }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Decision(string ic, string parent, int payment)
        {
           

            IEnumerable<Payment_FileViewModel> PaymentDetails = (from objPayment in objAceTuitionEntities.tb_payment
                                                                 where (objPayment.payment_id == payment)
                                                                 join objReceipt in objAceTuitionEntities.tb_receipt on objPayment.payment_id equals objReceipt.payment_id
                                                                 join objEarly in objAceTuitionEntities.tb_early_payment on objReceipt.early_id equals objEarly.early_id
                                                                 join objChild in objAceTuitionEntities.tb_child on objReceipt.child_ic equals objChild.child_ic
                                                                 join objParent in objAceTuitionEntities.tb_parent on objChild.child_parent_ic equals objParent.parent_ic
                                                                 where (objParent.parent_ic == parent)


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

                                                                 let paymentFile = from objPaymentFile in objAceTuitionEntities.tb_payment_file
                                                                                    where (objPaymentFile.payment_id == payment)
                                                                                    select new PaymentFileViewModel()
                                                                                    {
                                                                                        payment_file_id = objPaymentFile.payment_file_id,
                                                                                        payment_file = objPaymentFile.payment_file,
                                                                                        payment_status = objPayment.payment_status,
                                                                                        payment_date = objPayment.payment_date
                                                                                    }


                                                                 //join objPaymetFile in objAceTuitionEntities.tb_payment_file on objPayment.payment_id equals objPaymetFile.payment_id
                                                                         
                                                                         orderby objPayment.payment_year, objPayment.payment_month, objChild.child_code
                                                                         select new Payment_FileViewModel()
                                                                         {
                                                                             payment_id = objPayment.payment_id,
                                                                             payment_date = objPayment.payment_date,
                                                                             payment_month = objPayment.payment_month,
                                                                             payment_year = objPayment.payment_year,
                                                                             payment_status = objPayment.payment_status,
                                                                             payment_upload_date = objPayment.payment_upload_date,
                                                                             payment_balance_amount = (decimal)objPayment.payment_balance_amount,
                                                                             parent_ic = objPayment.parent_ic,
                                                                             payment_outstanding = objPayment.payment_outstanding,
                                                                             child_ic = objChild.child_ic,
                                                                             early_id = objReceipt.early_id,
                                                                             early_english_name = objEarly.early_english_name,
                                                                             early_chinese_name = objEarly.early_chinese_name,
                                                                             early_day = objEarly.early_day,
                                                                             early_operation = objEarly.early_operation,
                                                                             early_status = objEarly.early_status,
                                                                             early_value = objEarly.early_value,
                                                                             child_name_eng = objChild.child_name_eng,
                                                                             child_name_chinese = objChild.child_name_chinese,
                                                                             child_code = objChild.child_code,
                                                                             parent_name = objParent.parent_name,
                                                                             payment_amount = objPayment.payment_amount,
                                                                             child_trans_day = objChild.child_trans_day,
                                                                             child_num_subject = objChild.child_num_subject,
                                                                             receipt_addonList = receiptAddon,
                                                                             receipt_packageList = receiptPackage,
                                                                             receipt_discountList = receiptDiscount,
                                                                             child_childsubject_list = childSubject,
                                                                             //payment_file = objPaymetFile.payment_file
                                                                             payment_file_list = paymentFile
                                                                         }).ToList();

            IEnumerable<EarlypaymentViewModel> EarlyPayment = (from objEarly in objAceTuitionEntities.tb_early_payment
                                                               where (objEarly.early_id == 1 || objEarly.early_id == 3)
                                                               select new EarlypaymentViewModel()
                                                               {
                                                                   early_id = objEarly.early_id,
                                                                   early_chinese_name = objEarly.early_chinese_name,
                                                                   early_english_name = objEarly.early_english_name,
                                                                   early_value = objEarly.early_value,
                                                                   early_day = objEarly.early_day,
                                                                   early_status = objEarly.early_status,
                                                                   early_operation = objEarly.early_operation
                                                               }).ToList();


            IEnumerable<BalanceViewModel> AccBalance = (from objBalance in objAceTuitionEntities.tb_balance
                                                        join objParent in objAceTuitionEntities.tb_parent on objBalance.parent_ic equals objParent.parent_ic
                                                        where (objBalance.status == "pending" && objBalance.parent_ic == parent)
                                                        select new BalanceViewModel()
                                                        {
                                                            parent_ic = objBalance.parent_ic,
                                                            balance_id = objBalance.balance_id,
                                                            balance_amount = objBalance.balance_amount,
                                                            month = objBalance.month,
                                                            status = objBalance.status,

                                                        }).ToList();

            int count = 0;
            var pending = objAceTuitionEntities.tb_payment.Where(x => x.payment_status == "pending").ToList();
            foreach (var p in pending)
            {
                count++;
            }
            TempData["count"] = count;


            var mix = new DecisionViewModel();

            mix.PaymentList = PaymentDetails;
            mix.earlyList = EarlyPayment;
            mix.accBalance = AccBalance;


            return View(mix);

          

        }


        [HttpPost]
        public ActionResult AdminPay(PaymentViewModel objPaymentViewModel)
        {
            using (var context = new AceTuitionEntities())
            {

                var payment = context.tb_payment.Where(x => x.payment_id == objPaymentViewModel.payment_id).SingleOrDefault();
                var receipt = context.tb_receipt.Where(x => x.payment_id == objPaymentViewModel.payment_id).SingleOrDefault();

                receipt.early_id = 6;

                decimal grandTotal = payment.payment_outstanding;

                var balanceList = context.tb_balance.Where(x => x.parent_ic == payment.parent_ic && x.status == "active").ToList();
                foreach (var balance in balanceList)
                {
                    grandTotal = grandTotal - balance.balance_amount;
                    payment.payment_balance_amount = balance.balance_amount;
                }


                decimal totalEarlyDiscount = 0;

                var recPackge = context.tb_receipt_package.Where(x => x.receipt_id == receipt.receipt_id).ToList();

                var childcareEarly = false;
                var tuitionEarly = false;

                var childcareEarlyChecked = false;
                var tutionEarlyChecked = false;


                foreach (var rPackage in recPackge)
                {
                    var package = context.tb_package.Where(x => x.package_id == rPackage.package_id).SingleOrDefault();
                    if (package.package_operation == "Childcare")
                    {
                        childcareEarly = true;
                    }

                    if (package.package_operation == "Tuition")
                    {
                        tuitionEarly = true;
                    }
                }


                if (childcareEarly)
                {
                    var earlyPackage = context.tb_early_payment.Where(x => x.early_id == 3 && x.early_status == "active").SingleOrDefault();
                    if (earlyPackage != null)
                    {
                        if (earlyPackage.early_day >= objPaymentViewModel.payment_date.Day)
                        {
                            totalEarlyDiscount = totalEarlyDiscount + earlyPackage.early_value;
                            receipt.early_id = earlyPackage.early_id;
                            childcareEarlyChecked = true;
                        }

                    }
                }


                if (tuitionEarly)
                {
                    var earlyPackage = context.tb_early_payment.Where(x => x.early_id == 1 && x.early_status == "active").SingleOrDefault();
                    if (earlyPackage != null)
                    {
                        if (earlyPackage.early_day >= objPaymentViewModel.payment_date.Day)
                        {
                            totalEarlyDiscount = totalEarlyDiscount + earlyPackage.early_value;
                            receipt.early_id = earlyPackage.early_id;
                            tutionEarlyChecked = true;
                        }

                    }
                }



                if (tutionEarlyChecked && childcareEarlyChecked)
                {
                    receipt.early_id = 5;
                }


                grandTotal = grandTotal - totalEarlyDiscount;




                if (objPaymentViewModel.payment_amount < grandTotal)
                {
                    TempData["message"] = "Payment amount is insufficient";
                    //return Json(data: new { Success = false, Message = "Insufficient payment" }, JsonRequestBehavior.AllowGet);
                    return RedirectToAction("Index", "Outstanding");
                }



                //if(payment.payment_outstanding)

                payment.payment_date = objPaymentViewModel.payment_date;
                payment.payment_upload_date = DateTime.Now;
                payment.payment_amount = objPaymentViewModel.payment_amount;
                //payment.payment_outstanding = grandTotal;
                payment.payment_status = "accepted";
                payment.admin_ic = Session["ic"].ToString();
                payment.payment_decision_date = DateTime.Now;

                receipt.receipt_outstanding = grandTotal;
                receipt.receipt_status = "accepted";

                string[] monthName = { "JAN", "FEB", "MAR", "APR", "MAY", "JUN", "JUL", "AUG", "SEP", "OCT", "NOV", "DEC" };
                var childInfo = context.tb_child.Where(x => x.child_ic == payment.child_ic).SingleOrDefault();

                var countPayment = context.tb_payment.Count(t => t.payment_status == "accepted" && t.payment_month == DateTime.Now.Month) + 1;

                if (Int16.Parse(childInfo.child_year) < 7)
                {
                    var recPackage = context.tb_receipt_package.Where(x => x.receipt_id == receipt.receipt_id).ToList();
                    var childPackageFound = false;
                    foreach(var rp in recPackage)
                    {
                        var package = context.tb_package.Where(x => x.package_id == rp.package_id).SingleOrDefault();
                        if (package.package_attribute == "Childcare")
                        {
                            childPackageFound = true;
                        }
                    }

                    if(childPackageFound)
                    {
                        receipt.receipt_code = monthName[DateTime.Now.Month - 1] + "PC" + countPayment.ToString().PadLeft(4, '0');
                    } else
                    {
                        receipt.receipt_code = monthName[DateTime.Now.Month - 1] + "P" + countPayment.ToString().PadLeft(4, '0');
                    }
                    
                } else
                {
                    receipt.receipt_code = monthName[DateTime.Now.Month - 1] + "S" + countPayment.ToString().PadLeft(4, '0');
                }
                

                //receipt.receipt_outstanding = grandTotal;


                if (objPaymentViewModel.receipt != null)
                {
                    foreach (HttpPostedFileBase file in objPaymentViewModel.receipt)
                    {
                        //Checking file is available to save.  
                        if (file != null)
                        {
                            //var InputFileName = Path.GetFileName(file.FileName);
                            var InputFileName = Guid.NewGuid() + Path.GetExtension(file.FileName);
                            //var ServerSavePath = Path.Combine(Server.MapPath("~/UploadReceipt/") + InputFileName);
                            var ServerSavePath = Server.MapPath("~/UploadReceipt/" + InputFileName);
                            //Save file to server folder  


                            file.SaveAs(ServerSavePath);


                            //assigning file uploaded status to ViewBag for showing message to user.  
                            //ViewBag.UploadStatus = files.Count().ToString() + " files uploaded successfully.";


                            tb_payment_file objFile = new tb_payment_file();
                            objFile.payment_id = payment.payment_id;
                            objFile.payment_file = "~/UploadReceipt/" + InputFileName;
                            objAceTuitionEntities.tb_payment_file.Add(objFile);
                            objAceTuitionEntities.SaveChanges();
                        }

                    }
                }


                if (objPaymentViewModel.payment_amount > grandTotal)
                {
                    tb_balance balance = new tb_balance();
                    balance.parent_ic = payment.parent_ic;
                    balance.balance_amount = objPaymentViewModel.payment_amount - grandTotal;
                    balance.month = payment.payment_month;
                    balance.status = "active";
                    objAceTuitionEntities.tb_balance.Add(balance);
                }

                var addonList = context.tb_addon.Where(x => x.child_ic == payment.child_ic && x.addon_status == "active" && x.addon_recursive == "false").ToList();
                foreach(var addon in addonList)
                {
                    addon.addon_status = "inactive";
                }

                var discountList = context.tb_discount.Where(x => x.child_ic == payment.child_ic && x.discount_status == "active" && x.discount_recursive == "false").ToList();
                foreach(var discount in discountList)
                {
                    discount.discount_status = "inactive";
                }

                foreach(var balance in balanceList)
                {
                    balance.status = "inactive";
                }
                

                objAceTuitionEntities.SaveChanges();
               context.SaveChanges();


                return RedirectToAction("Index", "Outstanding");
            }



            


        }




        [HttpPost]
        public JsonResult ApprovePayment(PaymentFileViewModel objPaymentFileViewModel)
        {
            using (var context = new AceTuitionEntities())
            {

                var payment = context.tb_payment.Where(x => x.payment_id == objPaymentFileViewModel.payment_id).SingleOrDefault();
                payment.payment_status = "accepted";
                payment.payment_decision_date = DateTime.Now;
                payment.admin_ic = Session["ic"].ToString();

                var receipt = context.tb_receipt.Where(x => x.payment_id == objPaymentFileViewModel.payment_id).SingleOrDefault();
                var early  = context.tb_early_payment.Where(x => x.early_id == receipt.early_id).SingleOrDefault();


                //var Parentbalance = context.tb_balance.Where(x => x.parent_ic == payment.parent_ic && x.status=="pending").SingleOrDefault();

                //decimal total_payable = 0;
                //if (Parentbalance != null)
                //{
                //    total_payable = payment.payment_outstanding - early.early_value - Parentbalance.balance_amount;
                //    payment.payment_balance_amount = Parentbalance.balance_amount;
                //} else
                //{
                //    total_payable = payment.payment_outstanding - early.early_value;
                //    payment.payment_balance_amount = 0;
                //}

                decimal total_payable = 0;
                total_payable = payment.payment_outstanding - early.early_value - (decimal)payment.payment_balance_amount;


                if (payment.payment_amount > total_payable)
                {
                    tb_balance balance = new tb_balance();
                    balance.balance_amount = payment.payment_amount - total_payable;
                    balance.parent_ic = payment.parent_ic;
                    balance.month = payment.payment_month;
                    balance.status = "active";

                    objAceTuitionEntities.tb_balance.Add(balance);

                }

                var addonList = context.tb_addon.Where(x => x.child_ic == payment.child_ic && x.addon_status == "active" && x.addon_recursive == "false").ToList();
                foreach (var addon in addonList)
                {
                    addon.addon_status = "inactive";
                }

                var discountList = context.tb_discount.Where(x => x.child_ic == payment.child_ic && x.discount_status == "active" && x.discount_recursive == "false").ToList();
                foreach (var discount in discountList)
                {
                    discount.discount_status = "inactive";
                }

                var balanceList = context.tb_balance.Where(x => x.parent_ic == payment.parent_ic && x.status == "pending").ToList();
                foreach (var balance in balanceList)
                {
                    balance.status = "inactive";
                }





                string[] monthName = { "JAN", "FEB", "MAR", "APR", "MAY", "JUN", "JUL", "AUG", "SEP", "OCT", "NOV", "DEC" };
                var childInfo = context.tb_child.Where(x => x.child_ic == payment.child_ic).SingleOrDefault();

                var countPayment = context.tb_payment.Count(t => t.payment_status == "accepted" && t.payment_month == DateTime.Now.Month) + 1;

                if (Int16.Parse(childInfo.child_year) < 7)
                {
                    var recPackage = context.tb_receipt_package.Where(x => x.receipt_id == receipt.receipt_id).ToList();
                    var childPackageFound = false;
                    foreach (var rp in recPackage)
                    {
                        var package = context.tb_package.Where(x => x.package_id == rp.package_id).SingleOrDefault();
                        if (package.package_attribute == "Childcare")
                        {
                            childPackageFound = true;
                        }
                    }

                    if (childPackageFound)
                    {
                        receipt.receipt_code = monthName[DateTime.Now.Month - 1] + "PC" + countPayment.ToString().PadLeft(4, '0');
                    }
                    else
                    {
                        receipt.receipt_code = monthName[DateTime.Now.Month - 1] + "P" + countPayment.ToString().PadLeft(4, '0');
                    }

                }
                else
                {
                    receipt.receipt_code = monthName[DateTime.Now.Month - 1] + "S" + countPayment.ToString().PadLeft(4, '0');
                }


                receipt.receipt_status = "accepted";
                receipt.receipt_outstanding = total_payable;
                objAceTuitionEntities.SaveChanges();
                context.SaveChanges();

                return Json(data: new { Success = true }, JsonRequestBehavior.AllowGet);
            }



                
        }



        [HttpPost]
        public JsonResult RejectPayment(PaymentFileViewModel objPaymentFileViewModel)
        {
            using (var context = new AceTuitionEntities())
            {

                var payment = context.tb_payment.Where(x => x.payment_id == objPaymentFileViewModel.payment_id).SingleOrDefault();

                payment.payment_status = "rejected";
                payment.payment_decision_date = DateTime.Now;
                payment.admin_ic = Session["ic"].ToString();

                var receipt = context.tb_receipt.Where(x => x.payment_id == objPaymentFileViewModel.payment_id).SingleOrDefault();
                receipt.receipt_status = "rejected";

                if (payment.payment_amount > payment.payment_outstanding)
                {
                    var balance = context.tb_balance.Where(x => x.parent_ic == payment.parent_ic && x.status == "pending").SingleOrDefault();
                    balance.status = "active";
                }





                tb_receipt newReceipt = new tb_receipt();
                tb_payment newPayment = new tb_payment();


                newPayment.payment_date = DateTime.Now;
                newPayment.payment_upload_date = DateTime.Now;
                newPayment.payment_status = "outstanding";
                newPayment.payment_amount = 0;
                newPayment.payment_month = payment.payment_month;
                newPayment.payment_year = payment.payment_year;
                newPayment.payment_balance_amount = 0;

                newPayment.parent_ic = payment.parent_ic;
                newPayment.admin_ic = Session["ic"].ToString();
                newPayment.child_ic = payment.child_ic;
                newPayment.payment_outstanding = payment.payment_outstanding;


                objAceTuitionEntities.tb_payment.Add(newPayment);
                objAceTuitionEntities.SaveChanges();

                int paymentID = newPayment.payment_id;

                newReceipt.child_ic = payment.child_ic;
                newReceipt.receipt_status = "outstanding";
                newReceipt.payment_id = paymentID;
                newReceipt.receipt_outstanding = payment.payment_outstanding;
                newReceipt.early_id = 6;


               objAceTuitionEntities.tb_receipt.Add(newReceipt);
               objAceTuitionEntities.SaveChanges();

                int receiptID = newReceipt.receipt_id;

                var recPackage = context.tb_receipt_package.Where(x => x.receipt_id == receipt.receipt_id).ToList();
                foreach(var rPackage in recPackage)
                {
                    rPackage.receipt_id = receiptID;
                    context.SaveChanges();
                }

                var recAddon = context.tb_receipt_addon.Where(x => x.receipt_id == receipt.receipt_id).ToList();
                foreach(var rAddon in recAddon)
                {
                    rAddon.receipt_id = receiptID;
                    context.SaveChanges();
                }

                var recDiscount = context.tb_receipt_discount.Where(x => x.receipt_id == receipt.receipt_id).ToList();
                foreach(var rDiscount in recDiscount)
                {
                    rDiscount.receipt_id = receiptID;
                  context.SaveChanges();
                }

                var balanceList = context.tb_balance.Where(x => x.parent_ic == payment.parent_ic && x.status == "pending").ToList();
                foreach (var balance in balanceList)
                {
                    balance.status = "active";
                }


                objAceTuitionEntities.SaveChanges();
                context.SaveChanges();

                return Json(data: new { Success = true }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult NotficationModal()
        {
            return PartialView();
        }


    }
}