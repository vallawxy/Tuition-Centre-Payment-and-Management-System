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
    public class ParentView_HistoryController : Controller
    {
        private AceTuitionEntities objAceTuitionEntities;
        AceTuitionEntities db = new AceTuitionEntities();

        public ParentView_HistoryController()
        {
            objAceTuitionEntities = new AceTuitionEntities();
        }

        // GET: ParentView_History
        public ActionResult Index()
        {
            var parent = Session["ic"];
            IEnumerable<PaymentViewModel> historyDetails = (from objPayment in objAceTuitionEntities.tb_payment
                                                            join objReceipt in objAceTuitionEntities.tb_receipt on objPayment.payment_id equals objReceipt.payment_id
                                                            join objParent in objAceTuitionEntities.tb_parent on objPayment.parent_ic equals objParent.parent_ic
                                                            join objChild in objAceTuitionEntities.tb_child on objPayment.child_ic equals objChild.child_ic
                                                            join objAdmin in objAceTuitionEntities.tb_admin on objPayment.admin_ic equals objAdmin.admin_ic
                                                            where (objPayment.payment_status != "outstanding" && objPayment.parent_ic == parent.ToString())
                                                            
                                                            let fileDetails = from objFile in objAceTuitionEntities.tb_payment_file
                                                                              where (objPayment.payment_id == objFile.payment_id)
                                                                              select new PaymentFileViewModel()
                                                                              {
                                                                                  payment_id = objFile.payment_id,
                                                                                  payment_file = objFile.payment_file,
                                                                                  payment_file_id = objFile.payment_file_id,
                                                                              }
                                                            orderby objPayment.payment_year descending, objPayment.payment_month descending, objPayment.payment_id descending
                                                            select new PaymentViewModel()
                                                            {
                                                                payment_id = objPayment.payment_id,
                                                                admin_ic = objPayment.admin_ic,
                                                                payment_amount = objPayment.payment_amount,
                                                                payment_date = objPayment.payment_date,
                                                                payment_decision_date = (DateTime?)objPayment.payment_decision_date ?? DateTime.Now,
                                                                payment_month = objPayment.payment_month,
                                                                payment_outstanding = objPayment.payment_outstanding,
                                                                payment_upload_date = objPayment.payment_upload_date,
                                                                payment_year = objPayment.payment_year,
                                                                payment_status = objPayment.payment_status,
                                                                child_ic = objPayment.child_ic,
                                                                receipt_status = objReceipt.receipt_status,
                                                                receipt_code=objReceipt.receipt_code,
                                                                child_code = objChild.child_code,
                                                                child_name_chinese = objChild.child_name_chinese,
                                                                child_name_eng = objChild.child_name_eng,
                                                                admin_name = objAdmin.admin_name,
                                                                paymentFileList = fileDetails,

                                                            }).ToList();
            return View(historyDetails);
        }
    }
}