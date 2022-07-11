using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AceTuitionPaymentSystem.Models;
using AceTuitionPaymentSystem.ViewModel;


namespace AceTuitionPaymentSystem.Controllers
{
    public class SubjectController : Controller
    {
        private AceTuitionEntities objAceTuitionEntities;
        AceTuitionEntities db = new AceTuitionEntities();
        public SubjectController()
        {
            objAceTuitionEntities = new AceTuitionEntities();
        }
        // GET: Subject
        public ActionResult Index()
        {
            IEnumerable<SubjectViewModel> listOfsubViewModel = (from objSubject in objAceTuitionEntities.tb_subject
                                                                orderby objSubject.subject_status
                                                                select new SubjectViewModel()
                                                                  {
                                                                      subject_id = objSubject.subject_id,
                                                                      subject_chinese_name = objSubject.subject_chinese_name,
                                                                      subject_english_name = objSubject.subject_english_name,
                                                                      subject_grade = objSubject.subject_grade,
                                                                      subject_status = objSubject.subject_status
                                                                  }).ToList();
            int count = 0;
            var pending = objAceTuitionEntities.tb_payment.Where(x => x.payment_status == "pending").ToList();
            foreach (var p in pending)
            {
                count++;
            }
            TempData["count"] = count;
            return View(listOfsubViewModel);
        }

      
        public ActionResult AddorEdit(int? id)
        {
            
            var subject =  db.tb_subject.Find(id)?? new tb_subject();
            return PartialView("AddorEdit", subject);
        }

        public ActionResult CreateOrUpdate(tb_subject subject) 
        {
            //if (ModelState.IsValid==true)
            //{
                if (subject.subject_id > 0)
                {

                    db.Entry(subject).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                    return Json(data: new { success = true, message = "Updated Successfully" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    subject.subject_status = "active";
                    db.tb_subject.Add(subject);
                    db.SaveChanges();
                    return Json(data: new { success = true, message = "Saved Successfully" }, JsonRequestBehavior.AllowGet);
                }
            //}


            //return Json(data: new { success = false, message = "Error happened" }, JsonRequestBehavior.AllowGet);
            //return View(subject);
        }

       
    }
}