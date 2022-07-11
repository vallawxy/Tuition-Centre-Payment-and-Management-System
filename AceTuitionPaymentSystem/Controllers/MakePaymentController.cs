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
    public class MakePaymentController : Controller
    {


        private AceTuitionEntities objAceTuitionEntities;
        AceTuitionEntities db = new AceTuitionEntities();

        public MakePaymentController()
        {
            objAceTuitionEntities = new AceTuitionEntities();
        }


        // GET: MakePayment
        public ActionResult Index()
        {
            var parent = Session["ic"];
            var today = DateTime.Now.Day;
            var month = DateTime.Now.Month;
            int Dday = Convert.ToInt32(today);


            

            using (var context = new AceTuitionEntities())
            {
                List<tb_payment> payment = context.tb_payment.Where(x => x.parent_ic == parent.ToString() && x.payment_status == "outstanding").ToList();
                {
                    foreach(var item in payment)
                    {
                        if(item.payment_month==month)
                        {


                            var receipt = context.tb_receipt.Where(x => x.payment_id == item.payment_id).SingleOrDefault();
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
                                    if (earlyPackage.early_day >= DateTime.Now.Day)
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
                                    if (earlyPackage.early_day >= DateTime.Now.Day)
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
                            
                        }
                    }

                }
                
                context.SaveChanges();

            }



            IEnumerable<PaymentViewModel> outstandingList = (from objPayment in objAceTuitionEntities.tb_payment
                                                             join objChild in objAceTuitionEntities.tb_child on objPayment.child_ic equals objChild.child_ic
                                                             join objParent in objAceTuitionEntities.tb_parent on objPayment.parent_ic equals objParent.parent_ic
                                                             join objReceipt in objAceTuitionEntities.tb_receipt on objPayment.payment_id equals objReceipt.payment_id
                                                             join objEarly in objAceTuitionEntities.tb_early_payment on objReceipt.early_id equals objEarly.early_id
                                                             where ((objPayment.payment_status == "outstanding" || objPayment.payment_status == "pending") && objParent.parent_ic==parent.ToString())
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
                                                                 parent_ic = objPayment.parent_ic,
                                                                 child_ic = objPayment.child_ic,
                                                                 child_name_eng = objChild.child_name_eng,
                                                                 child_code = objChild.child_code,
                                                                 child_name_chinese = objChild.child_name_chinese,
                                                                 parent_name = objParent.parent_name,
                                                                 parent_phone = objParent.parent_phone,
                                                                 early_id = objEarly.early_id,
                                                                 early_english_name = objEarly.early_english_name,
                                                                 early_chinese_name = objEarly.early_chinese_name,
                                                                 early_day = objEarly.early_day,
                                                                 early_status = objEarly.early_status,
                                                                 early_value = objEarly.early_value,

                                                             }).ToList();

            IEnumerable<BalanceViewModel> balanaceList = (from objBalance in objAceTuitionEntities.tb_balance
                                                             join objParent in objAceTuitionEntities.tb_parent on objBalance.parent_ic equals objParent.parent_ic
                                                             where (objBalance.status == "active" && objParent.parent_ic == parent.ToString())
                                                             select new BalanceViewModel()
                                                             {
                                                                 parent_ic = objBalance.parent_ic,
                                                                 balance_id = objBalance.balance_id,
                                                                 balance_amount = objBalance.balance_amount,
                                                                 month = objBalance.month,
                                                                 status = objBalance.status,
                                                               
                                                             }).ToList();


            IEnumerable<EarlypaymentViewModel> earlyList = (from objReceipt in objAceTuitionEntities.tb_receipt
                                                            join objEarly in objAceTuitionEntities.tb_early_payment on objReceipt.early_id equals objEarly.early_id
                                                            join objPayment in objAceTuitionEntities.tb_payment on objReceipt.payment_id equals objPayment.payment_id
                                                            join objParent in objAceTuitionEntities.tb_parent on objPayment.parent_ic equals objParent.parent_ic
                                                            where ((objPayment.payment_status == "outstanding" || objPayment.payment_status == "pending") && objEarly.early_day>=Dday && objReceipt.early_id!=6 && objPayment.parent_ic == parent.ToString())
                                                            select new EarlypaymentViewModel()
                                                            {
                                                                early_id = objEarly.early_id,
                                                                early_english_name = objEarly.early_english_name,
                                                                early_chinese_name = objEarly.early_chinese_name,
                                                                early_day = objEarly.early_day,
                                                                early_operation = objEarly.early_operation,
                                                                early_status = objEarly.early_status,
                                                                early_value = objEarly.early_value,
                                                            }).Distinct().ToList();

            var mixmodel = new ParentMake_PaymentViewModel();

            mixmodel.paymentList = outstandingList;
            mixmodel.balanceList = balanaceList;
            mixmodel.earlyList = earlyList;


            return View(mixmodel);
        }




        public ActionResult Details(string payment)
        {
            List<ReceiptViewModel> ReceiptDetailsList = new List<ReceiptViewModel>();

            //ReceiptViewModel objReceiptViewModel = new ReceiptViewModel();
            //var parent = Session["ic"];
            //string[] paymentList = objReceiptViewModel.payment_id_list.Split(",".ToCharArray());
            //string[] childList = objReceiptViewModel.child_id_list.Split(",".ToCharArray());

            //for (var i = 0; i < paymentList.Length; i++)
            //{

            //    var paymentId = Int16.Parse(paymentList[i]);
            //    var payment = context.tb_payment.Where(x => x.payment_id == paymentId).SingleOrDefault();
            //}
            string[] paymentString = payment.Split(",".ToCharArray());
            List<int> paymentIntList = new List<int>();
            foreach( var pay in paymentString)
            {
                paymentIntList.Add(Int16.Parse(pay));
            }

            int[] paymentIntArray = paymentIntList.ToArray();
            var parent = Session["ic"];

            IEnumerable<ReceiptViewModel> ReceiptDetails = (from objReceipt in objAceTuitionEntities.tb_receipt
                                                            join objPayment in objAceTuitionEntities.tb_payment on objReceipt.payment_id equals objPayment.payment_id
                                                            join objParent in objAceTuitionEntities.tb_parent on objPayment.parent_ic equals objParent.parent_ic
                                                            join objEarly in objAceTuitionEntities.tb_early_payment on objReceipt.early_id equals objEarly.early_id
                                                            join objChild in objAceTuitionEntities.tb_child on objReceipt.child_ic equals objChild.child_ic
                                                            //where (objReceipt.receipt_status == "outstanding" && objPayment.payment_id == payment)
                                                            where (objReceipt.receipt_status == "outstanding" && paymentIntArray.Contains((int)objPayment.payment_id))

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
                                                                //child_payment_list = parentChild

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
                                                             where (objBalance.status =="active" && objBalance.parent_ic == parent.ToString())
                                                               select new BalanceViewModel()
                                                               {
                                                                   parent_ic = objBalance.parent_ic,
                                                                   balance_id = objBalance.balance_id,
                                                                   balance_amount = objBalance.balance_amount,
                                                                   month = objBalance.month,
                                                                   status = objBalance.status,

                                                               }).ToList();

            var mixmodel = new MakePaymentViewModel();

            mixmodel.paymentList = ReceiptDetails;
            mixmodel.earlyList = EarlyPayment;
            mixmodel.accBalance = AccBalance;


            return View(mixmodel);

        }


        //GET: MakePayment/Create
        public ActionResult Create(string payment, string child)
        {

            return PartialView();
        }

        
        // POST: MakePayment/Create
        [HttpPost]
        public ActionResult Create(PaymentViewModel objPaymentViewModel)
        {
            using (var context = new AceTuitionEntities())
            {

                string[] paymentList = objPaymentViewModel.payment_id_list.Split(",".ToCharArray());
                string[] childList = objPaymentViewModel.child_id_list.Split(",".ToCharArray());

                //string[] paymentList = payment.Split(",".ToCharArray());
                //string[] childList = child.Split(",".ToCharArray());


                // check outstanding total amount
                decimal totalPayableAmount = 0;
                decimal balanceAvailable = 0;
                decimal previousBalance = 0;

                for (var i = 0; i < paymentList.Length; i++)
                {

                    var paymentId = Int16.Parse(paymentList[i]);
                    var paymentContext = context.tb_payment.Where(x => x.payment_id == paymentId).SingleOrDefault();

                    totalPayableAmount = totalPayableAmount + paymentContext.payment_outstanding;

                    var receipt = context.tb_receipt.Where(x => x.payment_id == paymentId).SingleOrDefault();
                    var recPackage = context.tb_receipt_package.Where(x => x.receipt_id == receipt.receipt_id).ToList();

                    var childcareEarly = false;
                    var tuitionEarly = false;

                    foreach(var rpackage in recPackage)
                    {
                        var package = context.tb_package.Where(x => x.package_id == rpackage.package_id).SingleOrDefault();
                        if (package.package_operation == "Childcare")
                        {
                            childcareEarly = true;
                        }
                        if (package.package_operation == "Tuition")
                        {
                            tuitionEarly = true;
                        }
                    }

                    if(childcareEarly)
                    {
                        var earlyPackage = context.tb_early_payment.Where(x => x.early_id == 3 && x.early_status == "active").SingleOrDefault();
                        if (earlyPackage != null)
                        {
                            if (earlyPackage.early_day >= objPaymentViewModel.payment_date.Day)
                            {
                                totalPayableAmount = totalPayableAmount - earlyPackage.early_value;
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
                                totalPayableAmount = totalPayableAmount - earlyPackage.early_value;
                            }

                        }
                    }

                    if (i == 0)
                    {
                        var balanceList = context.tb_balance.Where(x => x.parent_ic == paymentContext.parent_ic && x.status == "active").ToList();
                        foreach (var balance in balanceList)
                        {
                            totalPayableAmount = totalPayableAmount - balance.balance_amount;
                            previousBalance = previousBalance + balance.balance_amount;
                        }

                    }

                }


                if (totalPayableAmount > objPaymentViewModel.payment_amount)
                {
                    TempData["message"] = "Payment amount is insufficient";
                    return RedirectToAction("Index", "MakePayment");
                    //return Json(data: new { Success = false, Message = "Payment amount insufficient." }, JsonRequestBehavior.AllowGet);
                }


                if(totalPayableAmount < objPaymentViewModel.payment_amount)
                {
                    balanceAvailable = objPaymentViewModel.payment_amount - totalPayableAmount;
                }











                List<string> filenameList = new List<string>();

                foreach (HttpPostedFileBase file in objPaymentViewModel.receipt)
                {
                    //Checking file is available to save.  
                    if (file != null)
                    {
                        
                        var InputFileName = Guid.NewGuid() + Path.GetExtension(file.FileName);
                        var ServerSavePath = Server.MapPath("~/UploadReceipt/" + InputFileName);
                        filenameList.Add("~/UploadReceipt/" + InputFileName);
                        file.SaveAs(ServerSavePath);
                        
                    }

                }



                string[] filenameArray = filenameList.ToArray();


               

                for (var i = 0; i < paymentList.Length; i++)
                {
                    var paymentId = Int16.Parse(paymentList[i]);
                    var paymentContext = context.tb_payment.Where(x => x.payment_id == paymentId).SingleOrDefault();
                    paymentContext.payment_date = objPaymentViewModel.payment_date;
                    paymentContext.payment_upload_date = DateTime.Now;
                    paymentContext.payment_amount = objPaymentViewModel.payment_amount;
                    paymentContext.payment_status = "pending";

                    if (previousBalance > 0 && i == 0)
                    {
                        paymentContext.payment_balance_amount = previousBalance;

                    }

                    decimal tmpOut = paymentContext.payment_outstanding;
                    

                    foreach(var file in filenameArray) {
                        tb_payment_file objFile = new tb_payment_file();
                        objFile.payment_id = paymentId;
                        objFile.payment_file = file;
                        objAceTuitionEntities.tb_payment_file.Add(objFile);
                        objAceTuitionEntities.SaveChanges();
                    }

                    var rec = context.tb_receipt.Where(x => x.payment_id == paymentId).SingleOrDefault();
                    rec.receipt_status = "pending";

                    var recPackage = context.tb_receipt_package.Where(x => x.receipt_id == rec.receipt_id).ToList();

                    var tuitionEarly = false;
                    var childcareEarly = false;
                    var tutionEarlyChecked = false;
                    var childcareEarlyChecked = false;
                    decimal totalEarlyDiscount = 0;
                    foreach (var rpackage in recPackage)
                    {
                        var package = context.tb_package.Where(x => x.package_id == rpackage.package_id).SingleOrDefault();
                        if (package.package_operation == "Childcare")
                        {
                            childcareEarly = true;
                        }
                        if (package.package_operation == "Tuition")
                        {
                            tuitionEarly = true;
                        }
                    }

                    if(childcareEarly)
                    {
                        var earlyPackage = context.tb_early_payment.Where(x => x.early_id == 3 && x.early_status == "active").SingleOrDefault();
                        if(earlyPackage != null)
                        {
                            if (earlyPackage.early_day >= DateTime.Now.Day)
                            {
                                totalEarlyDiscount = totalEarlyDiscount + earlyPackage.early_value;
                                rec.early_id = earlyPackage.early_id;
                                childcareEarlyChecked = true;
                            }
                            
                        }
                    }


                    if (tuitionEarly)
                    {
                        var earlyPackage = context.tb_early_payment.Where(x => x.early_id == 1 && x.early_status == "active").SingleOrDefault();
                        if (earlyPackage != null)
                        {
                            if(earlyPackage.early_day >= DateTime.Now.Day)
                            {
                                totalEarlyDiscount = totalEarlyDiscount + earlyPackage.early_value;
                                rec.early_id = earlyPackage.early_id;
                                tutionEarlyChecked = true;
                            }
                            
                        }
                    }

                    if (tutionEarlyChecked && childcareEarlyChecked)
                    {
                        rec.early_id = 5;
                    }


                    if(rec.early_id != 6)
                    {
                        tmpOut = tmpOut - totalEarlyDiscount;
                    }

                    //if (i == 0)
                    //{
                    //    tmpOut = tmpOut - previousBalance;
                    //}


                    

                    if (i == 0 && balanceAvailable > 0)
                    {
                        paymentContext.payment_amount = tmpOut + balanceAvailable - previousBalance;
                    } else
                    {
                        paymentContext.payment_amount = tmpOut;
                    }

                        //if (i == 0 && balanceAvailable > 0)
                        //{

                        //    tb_balance balance = new tb_balance();
                        //    balance.parent_ic = payment.parent_ic;
                        //    balance.balance_amount = balanceAvailable;
                        //    balance.month = payment.payment_month;
                        //    balance.status = "active";

                        //    objAceTuitionEntities.tb_balance.Add(balance);
                        //    objAceTuitionEntities.SaveChanges();
                        //}


                    var balanceList = context.tb_balance.Where(x => x.parent_ic == paymentContext.parent_ic && x.status == "active").ToList();
                    foreach (var balance in balanceList)
                    {
                        balance.status = "pending";
                    }




                    context.SaveChanges();

                    


                }


                return RedirectToAction("Index", "MakePayment");

            }


            
        }
    }
}
