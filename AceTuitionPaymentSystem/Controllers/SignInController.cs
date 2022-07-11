using AceTuitionPaymentSystem.Models;
using AceTuitionPaymentSystem.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AceTuitionPaymentSystem.Controllers
{
    public class SignInController : Controller
    {
        //
        // GET: /Login/
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Autherize(AdminViewModel userModel)
        {
            using (AceTuitionEntities db = new AceTuitionEntities())
            {
                var userDetails = db.tb_admin.Where(x => x.admin_ic == userModel.admin_ic && x.admin_password == userModel.admin_password).FirstOrDefault();
                if (userDetails == null)
                {
                    userModel.LoginErrorMessage = "Wrong username or password.";
                    return View("Index", userModel);
                }
                else
                {
                    Session["admin_ic"] = userDetails.admin_ic;
                    Session["admin_name"] = userDetails.admin_name;
                    return RedirectToAction("Index", "Parents");
                }
            }
        }

        //public ActionResult Autherize(ParentsViewModel userModel)
        //{
        //    using (AceTuitionEntities db = new AceTuitionEntities())
        //    {
        //        var userDetails = db.tb_parent.Where(x => x.parent_ic == userModel.parent_ic && x.parent_password == userModel.parent_password).FirstOrDefault();
        //        if (userDetails == null)
        //        {
        //            userModel.LoginErrorMessage = "Wrong username or password.";
        //            return View("Index", userModel);
        //        }
        //        else
        //        {
        //            Session["parent_ic"] = userDetails.parent_ic;
        //            Session["parent_password"] = userDetails.parent_password;
        //            return RedirectToAction("Index", "Child");
        //        }
        //    }
        //}
        public ActionResult LogOut()
        {
            string admin_ic = (string)Session["admin_ic"];
            string parent_ic = (string)Session["parent_ic"];
            Session.Abandon();
            return RedirectToAction("Index", "SignIn");
        }



    }
}