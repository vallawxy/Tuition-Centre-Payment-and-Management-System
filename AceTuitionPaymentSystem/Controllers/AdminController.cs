using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AceTuitionPaymentSystem.Models;
using AceTuitionPaymentSystem.ViewModel;
using AceTuitionPaymentSystem.Password;

namespace AceTuitionPaymentSystem.Controllers
{
    public class AdminController : Controller
    {
        // GET: Admin
        private AceTuitionEntities objAceTuitionEntities;
        AceTuitionEntities db = new AceTuitionEntities();
        public AdminController()
        {
            objAceTuitionEntities = new AceTuitionEntities();
        }


        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Create(tb_admin admin)
        {
            //parents.parent_password = parents.parent_ic;
            admin.admin_status = "active";

            admin.admin_password = Hashing.HashPassword(admin.admin_ic);
            admin.admin_new_password = admin.admin_password;
            admin.admin_confirm_password = admin.admin_new_password;


            //admin.admin_password = admin.admin_ic;

            var data = db.tb_admin.Where(x => x.admin_ic == admin.admin_ic).SingleOrDefault();
            if (data == null)
            {

                db.tb_admin.Add(admin);
               db.SaveChanges();
                return Json(data: new { success = true, message = "Saved Successfully" }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(data: new { success = false, message = "Admin already created." }, JsonRequestBehavior.AllowGet);

            }

        }

        public ActionResult EditAdmin(string id)
        {

            var admin = db.tb_admin.Find(id) ?? new tb_admin();
            return PartialView("EditAdmin", admin);
        }

        public ActionResult CreateAdmin(string id)
        {

            var admin = db.tb_admin.Find(id) ?? new tb_admin();
            return PartialView("CreateAdmin", admin);
        }


        public ActionResult UpdateAdmin(tb_admin admin)
        {
            if (admin.admin_new_password != null)
            {
                if (admin.admin_confirm_password.Length >= 6 && admin.admin_new_password.Length >= 6)
                {
                    if (admin.admin_new_password == admin.admin_new_password)
                    {
                        admin.admin_password = Hashing.HashPassword(admin.admin_new_password);
                    }
                    else
                    {
                        return Json(data: new { success = false, message = "Password is not matched." }, JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    return Json(data: new { success = false, message = "Password required minimum 6 digits." }, JsonRequestBehavior.AllowGet);
                }

                
            }
            Session["name"] = admin.admin_name;
            db.Entry(admin).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
            return Json(data: new { success = true, message = "Updated Successfully" }, JsonRequestBehavior.AllowGet);

        }

    }
}