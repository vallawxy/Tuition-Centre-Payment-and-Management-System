using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AceTuitionPaymentSystem.Models;
using AceTuitionPaymentSystem.ViewModel;
using AceTuitionPaymentSystem.Password;

namespace AceTuitionPaymentSystem.Controllers
{
    public class LoginController : Controller
    {
        private AceTuitionEntities objEntities = new AceTuitionEntities();
        private List<AdminViewModel> listOfAdminViewModel;

        public LoginController()
        {
            objEntities = new AceTuitionEntities();
            listOfAdminViewModel = new List<AdminViewModel>();
 
        }

        // GET: Login
       
        public ActionResult Index()
        {
      
            return View();
        }


        [HttpPost]
        public ActionResult Index(string userName,string password)
        {
            using (var context = new AceTuitionEntities())
            {

                var adminItem = context.tb_admin.Where(x => x.admin_ic == userName).SingleOrDefault();
                //var adminItem = context.tb_admin.Where(x => x.admin_ic == userName && x.admin_status == "active").SingleOrDefault();
                if (adminItem != null)
                {
                    if (Hashing.ValidatePassword(password, adminItem.admin_password))
                    {
                        Session["type"] = "admin";
                        Session["name"] = adminItem.admin_name.ToString();
                        Session["ic"] = adminItem.admin_ic.ToString();
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        TempData["message"] = "Invalid username or password";
                        return RedirectToAction("Index", "Login");

                    }

                }
                else
                {
                    var parentItem = context.tb_parent.Where(x => x.parent_ic == userName &&x.parent_status=="active").SingleOrDefault();
                    if (parentItem != null)

                    {
                        if (Hashing.ValidatePassword(password, parentItem.parent_password))
                        {
                            Session["type"] = "parent";
                            Session["name"] = parentItem.parent_name.ToString();
                            Session["ic"] = parentItem.parent_ic.ToString();
                            return RedirectToAction("Index", "ParentView");
                        }
                        else
                        {
                            TempData["message"] = "Invalid username or password";
                            return RedirectToAction("Index", "Login");

                        }

                    }
                    else
                    {
                        TempData["message"] = "Invalid username or password";
                        return RedirectToAction("Index", "Login");
                       
                    }
                    
                }
                //return View("Index");
            }
        }
        public ActionResult AfterLogin()
        {
            if (Session["type"] != null)
            {
                return View();
            }
            else
            {
                return Redirect("Login");
            }
        }
        public ActionResult Logout()
        {
            Session.Clear();
            Session.Abandon();
            return RedirectToAction("Index", "Login");
        }

        public ActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ForgotPassword(string userName, string phone,string password,string confirmpassword)
        {
            using (var context = new AceTuitionEntities())
            {

                var adminItem = context.tb_admin.Where(x => x.admin_ic == userName).SingleOrDefault();
                //var adminItem = context.tb_admin.Where(x => x.admin_ic == userName && x.admin_status == "active").SingleOrDefault();
                if (adminItem != null)
                {
                    var adminPhone = context.tb_admin.Where(x => x.admin_phone_number == phone).SingleOrDefault();
                    if(adminPhone!=null)
                    {
                        if (password != null && confirmpassword != null)
                        {
                            if (password.Length >= 6 && confirmpassword.Length >= 6)
                            {
                                if (password == confirmpassword)
                                {
                                    adminItem.admin_password = Hashing.HashPassword(password);
                                    context.SaveChanges();
                                    TempData["success"] = "New Password is updated.";
                                    return RedirectToAction("Index", "Login");
                                }
                                else
                                {
                                    TempData["message"] = "Passwords are not matched.";
                                    return RedirectToAction("ForgotPassword", "Login");
                                }
                            }
                            else
                            {
                                TempData["message"] = "Minimum length of password must be 6";
                                return RedirectToAction("ForgotPassword", "Login");
                            }
                        }
                        else
                        {
                            TempData["message"] = "Password is required to fill.";
                            return RedirectToAction("ForgotPassword", "Login");
                        }
                    }
                    else
                    {
                        TempData["message"] = "Invalid phone number";
                        return RedirectToAction("ForgotPassword", "Login");

                    }

                }
                else { 
                
                  var parentItem = context.tb_parent.Where(x => x.parent_ic == userName && x.parent_status == "active").SingleOrDefault();
                    if (parentItem != null)
                    {
                        var parentPhone = context.tb_parent.Where(x => x.parent_phone == phone).FirstOrDefault();
                        if (parentPhone != null)
                        {
                            if (password != null && confirmpassword != null)
                            {
                                if (password.Length >= 6 && confirmpassword.Length >= 6)
                                {
                                    if (password == confirmpassword)
                                    {
                                        parentItem.parent_password = Hashing.HashPassword(password);
                                        context.SaveChanges();
                                        TempData["success"] = "New Password is updated.";
                                        return RedirectToAction("Index", "Login");
                                    }
                                    else
                                    {
                                        TempData["message"] = "Passwords are not matched.";
                                        return RedirectToAction("ForgotPassword", "Login");
                                    }
                                }
                                else
                                {
                                    TempData["message"] = "Minimum length of password must be 6";
                                    return RedirectToAction("ForgotPassword", "Login");
                                }
                            }
                            else
                            {
                                TempData["message"] = "Password is required to fill.";
                                return RedirectToAction("ForgotPassword", "Login");
                            }
                        }
                        else
                        {
                            TempData["message"] = "Invalid phone number";
                            return RedirectToAction("ForgotPassword", "Login");

                        }

                    }
                    else
                    {
                        TempData["message"] = "Invalid IC Number";
                        return RedirectToAction("ForgotPassword", "Login");

                    }

                }
               
                //return View("Index");
            }
        }
    }
}