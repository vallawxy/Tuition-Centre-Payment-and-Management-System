using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AceTuitionPaymentSystem.Models;
using AceTuitionPaymentSystem.ViewModel;

namespace AceTuitionPaymentSystem.Controllers
{
    public class ReceiptController : Controller
    {
        private AceTuitionEntities objAceTuitionEntities;
        AceTuitionEntities db = new AceTuitionEntities();
        // GET: Receipt
        public ReceiptController()
        {
            objAceTuitionEntities = new AceTuitionEntities();
        }
        public ActionResult Index()
        {
            IEnumerable<ReceiptViewModel> listOfReceiptViewModel = (from objReceipt in objAceTuitionEntities.tb_receipt
                                                                   
                                                                    select new ReceiptViewModel()
                                                                    {
                                                                        receipt_id = objReceipt.receipt_id,
                                                                        receipt_outstanding = objReceipt.receipt_outstanding,
                                                                        receipt_status = objReceipt.receipt_status,
                                                                        child_ic = objReceipt.child_ic,
                                                                        early_id = objReceipt.early_id,
                                                                        payment_id = objReceipt.payment_id

                                                                    }).ToList();
            return View(listOfReceiptViewModel);
        }

       [HttpPost]

       public JsonResult Index(string child_ic, List<string>listofchildic, List<string>listofparentic)
        {
            using (var context = new AceTuitionEntities())
            {
                

                for (var i = 0; i < listofchildic.Count; i++)
                {

                    tb_receipt receipt = new tb_receipt();
                    tb_payment payment = new tb_payment();

                    payment.payment_date = DateTime.Now;
                    payment.payment_upload_date = DateTime.Now;
                    payment.payment_status = "outstanding";
                    payment.payment_amount = 0;
                    payment.payment_month = DateTime.Now.Month;
                    payment.payment_year = DateTime.Now.Year;
                    payment.payment_balance_amount = 0;

                    payment.parent_ic = listofparentic[i];
                    payment.admin_ic = Session["ic"].ToString();
                    payment.child_ic = listofchildic[i];

                    decimal totalOutstanding = 0;

                    //-Check Previous Month
                    var temp_child_ic = listofchildic[i];
                    //var previousPaymentList = context.tb_payment.Where(x => x.child_ic == temp_child_ic && x.payment_status == "outstanding").ToList();
                    //foreach (var pay in previousPaymentList)
                    //{
                    //    totalOutstanding = totalOutstanding + pay.payment_outstanding;
                    //}

                    //-Get Package, Addon(active), Discount(active)
                    //-Outstanding Total for A Child = [Package] + [Addon] - [Discount]

                    var childPackageList = context.tb_child_package.Where(x => x.child_ic == temp_child_ic).ToList();
                    foreach (var package in childPackageList)
                    {
                        var packageValue = context.tb_package.Where(x => x.package_id == package.package_id).SingleOrDefault();
                        var childInfo = context.tb_child.Where(x => x.child_ic == temp_child_ic).SingleOrDefault();

                        if (packageValue.package_operation == "num_of_subject")
                        {
                            totalOutstanding = totalOutstanding + (packageValue.package_price * childInfo.child_num_subject);
                        }
                        else if(packageValue.package_operation == "num_of_transport_day")
                        {
                            totalOutstanding = totalOutstanding + (packageValue.package_price * childInfo.child_trans_day * 4);
                        }
                        else
                        {
                            totalOutstanding = totalOutstanding + packageValue.package_price;
                        }

                    }

                    var childAddonList = context.tb_addon.Where(x => x.child_ic == temp_child_ic && x.addon_status == "active").ToList();
                    foreach (var addon in childAddonList)
                    {
                        totalOutstanding = totalOutstanding + addon.addon_value;
                    }

                    var childDiscountList = context.tb_discount.Where(x => x.child_ic == temp_child_ic && x.discount_status == "active").ToList();
                    foreach (var discount in childDiscountList)
                    {
                        totalOutstanding = totalOutstanding - discount.discount_value;
                    }


                    payment.payment_outstanding = totalOutstanding;

                    objAceTuitionEntities.tb_payment.Add(payment);

                    //objAceTuitionEntities.SaveChanges();

                    int paymentID = payment.payment_id;

                    receipt.child_ic = listofchildic[i];
                    receipt.receipt_status = "outstanding";
                    receipt.payment_id = paymentID;
                    receipt.early_id = 6;
                    receipt.receipt_outstanding = totalOutstanding;


                    objAceTuitionEntities.tb_receipt.Add(receipt);

                    //objAceTuitionEntities.SaveChanges();


                    int receiptID = receipt.receipt_id;

                    foreach (var package in childPackageList)
                    {
                        tb_receipt_package recPackage = new tb_receipt_package();
                        var packageValue = context.tb_package.Where(x => x.package_id == package.package_id).SingleOrDefault();
                        recPackage.receipt_id = receiptID;
                        recPackage.package_id = packageValue.package_id;


                        var childInfo = context.tb_child.Where(x => x.child_ic == temp_child_ic).SingleOrDefault();

                        if (packageValue.package_operation == "num_of_subject")
                        {
                            recPackage.value = packageValue.package_price * childInfo.child_num_subject;
                        }
                        else if (packageValue.package_operation == "num_of_transport_day")
                        {
                            recPackage.value = packageValue.package_price * childInfo.child_trans_day * 4;
                        }
                        else
                        {
                            recPackage.value = packageValue.package_price;
                        }
                        
                        objAceTuitionEntities.tb_receipt_package.Add(recPackage);
                        //objAceTuitionEntities.SaveChanges();
                    }

                    foreach (var addon in childAddonList)
                    {
                        tb_receipt_addon recAddon = new tb_receipt_addon();
                        recAddon.receipt_id = receiptID;
                        recAddon.addon_id = addon.addon_id;
                        recAddon.value = addon.addon_value;
                        objAceTuitionEntities.tb_receipt_addon.Add(recAddon);
                        //objAceTuitionEntities.SaveChanges();
                    }

                    foreach (var discount in childDiscountList)
                    {
                        tb_receipt_discount recDiscount = new tb_receipt_discount();
                        recDiscount.receipt_id = receiptID;
                        recDiscount.discount_id = discount.discount_id;
                        recDiscount.value = discount.discount_value;
                        objAceTuitionEntities.tb_receipt_discount.Add(recDiscount);
                        //objAceTuitionEntities.SaveChanges();
                    }



                    try
                    {

                        objAceTuitionEntities.SaveChanges();
                    }
                    catch (DbEntityValidationException dbEx)
                    {
                        foreach (var validationErrors in dbEx.EntityValidationErrors)
                        {
                            foreach (var validationError in validationErrors.ValidationErrors)
                            {
                                System.Console.WriteLine("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage);
                            }
                        }
                    }


                    



                }



                return Json(data:new {success=true, message="Outstanding successfully generate to parents!"}, JsonRequestBehavior.AllowGet);

            }
            
           
        }
        public ActionResult Details(string ic,int payment_id)
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
                                                            //let earlyPayment = from objEarly in objAceTuitionEntities.tb_early_payment
                                                            //                   where (objEarly.early_id == objReceipt.early_id)
                                                            //                   select new EarlypaymentViewModel()
                                                            //                   {
                                                            //                       early_id = objEarly.early_id,
                                                            //                       early_english_name = objEarly.early_english_name,
                                                            //                       early_chinese_name = objEarly.early_chinese_name,
                                                            //                       early_value = objEarly.early_value,
                                                            //                       early_day = objEarly.early_day,
                                                            //                       early_status = objEarly.early_status,
                                                            //                       early_operation = objEarly.early_operation

                                                            //                   }
                                                            let Early = from objEarly in objAceTuitionEntities.tb_early_payment
                                                                        where (objEarly.early_id != 6 && objEarly.early_id != 5)
                                                                        select new EarlypaymentViewModel()
                                                                        {
                                                                            early_id = objEarly.early_id,
                                                                            early_chinese_name = objEarly.early_chinese_name,
                                                                            early_english_name = objEarly.early_english_name,
                                                                            early_value = objEarly.early_value
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
                                                                payment_outstanding=objPayment.payment_outstanding,
                                                                payment_balance_amount=(decimal)objPayment.payment_balance_amount,
                                                                receipt_code= objReceipt.receipt_code,
                                                                early_List = Early,
                                                            }).ToList();

            return PartialView(ReceiptDetails);
        }

    }
}