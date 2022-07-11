using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AceTuitionPaymentSystem.Models;
using AceTuitionPaymentSystem.ViewModel;


namespace AceTuitionPaymentSystem.Controllers
{
    public class earlypayController : Controller
    {
        private AceTuitionEntities objAceTuitionEntities;
        AceTuitionEntities db = new AceTuitionEntities();
        public earlypayController()
        {
            objAceTuitionEntities = new AceTuitionEntities();
        }
        // GET: earlypay
        public ActionResult Index()
        {
            IEnumerable<EarlypaymentViewModel> listOfearlypaymentViewModel = (from objEarlypayment in objAceTuitionEntities.tb_early_payment
                                                               
                                                                select new EarlypaymentViewModel()
                                                                {
                                                                    early_id = objEarlypayment.early_id,
                                                                    early_chinese_name = objEarlypayment.early_chinese_name,
                                                                    early_english_name = objEarlypayment.early_english_name,
                                                                    early_value = objEarlypayment.early_value,
                                                                    early_day = objEarlypayment.early_day,
                                                                    early_status = objEarlypayment.early_status,
                                                                    early_operation = objEarlypayment.early_operation
                                                                }).ToList(); 
            int count = 0;
            var pending = objAceTuitionEntities.tb_payment.Where(x => x.payment_status == "pending").ToList();
            foreach (var p in pending)
            {
                count++;
            }
            TempData["count"] = count;
            return View(listOfearlypaymentViewModel);
        }


        public ActionResult AddorEdit(int? id)
        {

            var earlypayment = db.tb_early_payment.Find(id) ?? new tb_early_payment();
            return PartialView("AddorEdit", earlypayment);
        }

        public ActionResult CreateOrUpdate(tb_early_payment earlypayment)
        {
            //if (ModelState.IsValid==true)
            //{
            if (earlypayment.early_id > 0)
            {

                db.Entry(earlypayment).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                using (var context = new AceTuitionEntities())
                {

                    var item5 = context.tb_early_payment.Where(x => x.early_id == 5).SingleOrDefault();
                    var item1 = context.tb_early_payment.Where(x => x.early_id == 1).SingleOrDefault();
                    var item3 = context.tb_early_payment.Where(x => x.early_id == 3).SingleOrDefault();

                    item5.early_value = item1.early_value + item3.early_value;
                    context.SaveChanges();

                }
               
                return Json(data: new { success = true, message = "Updated Successfully" }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                earlypayment.early_status = "active";
                earlypayment.early_operation = "Tuition";
                db.tb_early_payment.Add(earlypayment);
                db.SaveChanges();
                return Json(data: new { success = true, message = "Saved Successfully" }, JsonRequestBehavior.AllowGet);
            }
            //}


            //return Json(data: new { success = false, message = "Error happened" }, JsonRequestBehavior.AllowGet);
            //return View(subject);
        }
    }
}