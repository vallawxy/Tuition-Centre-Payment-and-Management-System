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
    public class noticeController : Controller
    {
        private AceTuitionEntities objAceTuitionEntities;
        AceTuitionEntities db = new AceTuitionEntities();
        public noticeController()
        {
            objAceTuitionEntities = new AceTuitionEntities();
        }
        // GET: notice
        public ActionResult Index()
        {
            IEnumerable<noticeViewModel> listOfnoticeViewModel = (from objNotice in objAceTuitionEntities.tb_notice
                                                                select new noticeViewModel()
                                                                {
                                                                    notice_id = objNotice.notice_id,
                                                                    notice_date = objNotice.notice_date,
                                                                    title = objNotice.title,
                                                                    description = objNotice.description,
                                       
                                                                }).ToList();
            int count = 0;
            var pending = objAceTuitionEntities.tb_payment.Where(x => x.payment_status == "pending").ToList();
            foreach(var p in pending)
            {
                count++;
            }
            TempData["count"] = count;
            return View(listOfnoticeViewModel);
        }

        
        public ActionResult Create(int? id)
        {

            var notice = db.tb_notice.Find(id) ?? new tb_notice();
            return PartialView("Create", notice);
        }

      

        public ActionResult CreateOrUpdate(tb_notice notice)
        {
            notice.notice_date = DateTime.Today;
          
        
            if (notice.notice_id > 0)
            {
                
                try
                {
                 
                    db.Entry(notice).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
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
                
                return Json(data: new { success = true, message = "Updated Successfully" }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                
               
                try
                {
                    db.tb_notice.Add(notice);
                    db.SaveChanges();
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

               
                return Json(data: new { success = true, message = "Saved Successfully" }, JsonRequestBehavior.AllowGet);
            }
           
          
        }

        // GET: notice/Edit/5
        
        //public ActionResult Delete(int id)
        //{
        //    return View();
        //}

        // POST: notice/Delete/5
        //[HttpPost]
        //public ActionResult Delete(int id, FormCollection collection)
        //{

        //        var myTableTest = db.tb_notice.Where(x => x.notice_id == id).FirstOrDefault();
        //        db.tb_notice.Remove(myTableTest);

        //        db.SaveChanges();
        //        return Json(true, JsonRequestBehavior.AllowGet);

        //}

        [HttpPost]
        public JsonResult Delete(noticeViewModel objNotice)
        {
            using (var context = new AceTuitionEntities())
            {

                var data = context.tb_notice.Where(x => x.notice_id == objNotice.notice_id).SingleOrDefault();
                if (data != null)
                {
                    context.tb_notice.Remove(data);
                    context.SaveChanges();
                    return Json(data: new { Success = true, message = "Delete Successfully" }, JsonRequestBehavior.AllowGet);
                }

                return Json(data: new { Success = false, message = "Delete Successfully" }, JsonRequestBehavior.AllowGet);

            }

        }

    }
}
