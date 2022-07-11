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
    public class ParentsController : Controller
    {
        // GET: Parents
        private AceTuitionEntities objAceTuitionEntities;
        AceTuitionEntities db = new AceTuitionEntities();
        public ParentsController()
        {
            objAceTuitionEntities = new AceTuitionEntities();
        }

        // GET: Parents
        public ActionResult Index(string status)
        {
            IEnumerable<ParentsViewModel> listOfParentsViewModel = null;

            if (status == "active")
            {
                listOfParentsViewModel = (from objParents in objAceTuitionEntities.tb_parent
                                          where (objParents.parent_status == "active")
                                          orderby objParents.parent_status, objParents.parent_name
                                          select new ParentsViewModel()
                                          {
                                              parent_ic = objParents.parent_ic,
                                              parent_password = objParents.parent_password,
                                              parent_name = objParents.parent_name,
                                              parent_address = objParents.parent_address,
                                              parent_phone = objParents.parent_phone,
                                              parent_status = objParents.parent_status,


                                          }).ToList();
            } else if (status == "inactive")
            {
                listOfParentsViewModel = (from objParents in objAceTuitionEntities.tb_parent
                                          where (objParents.parent_status == "inactive")
                                          orderby objParents.parent_status, objParents.parent_name
                                          select new ParentsViewModel()
                                          {
                                              parent_ic = objParents.parent_ic,
                                              parent_password = objParents.parent_password,
                                              parent_name = objParents.parent_name,
                                              parent_address = objParents.parent_address,
                                              parent_phone = objParents.parent_phone,
                                              parent_status = objParents.parent_status,


                                          }).ToList();
            } else
            {
                listOfParentsViewModel = (from objParents in objAceTuitionEntities.tb_parent
                                          orderby objParents.parent_status, objParents.parent_name
                                          select new ParentsViewModel()
                                          {
                                              parent_ic = objParents.parent_ic,
                                              parent_password = objParents.parent_password,
                                              parent_name = objParents.parent_name,
                                              parent_address = objParents.parent_address,
                                              parent_phone = objParents.parent_phone,
                                              parent_status = objParents.parent_status,


                                          }).ToList();
            }
            


            IEnumerable<BalanceViewModel> balanceList = (from objBalance in objAceTuitionEntities.tb_balance
                                                         where (objBalance.status == "active")
                                                         select new BalanceViewModel()
                                                         {
                                                             balance_amount = objBalance.balance_amount,
                                                             balance_id = objBalance.balance_id,
                                                             status = objBalance.status,
                                                             month = objBalance.month,
                                                             parent_ic=objBalance.parent_ic

                                                         }).ToList();

            
            


            int count = 0;
            var pending = objAceTuitionEntities.tb_payment.Where(x => x.payment_status == "pending").ToList();
            foreach (var p in pending)
            {
                count++;
            }
            TempData["count"] = count;

            var mixmodel = new ParentBalanceViewModel();

            mixmodel.parentList = listOfParentsViewModel;
            mixmodel.balanceList = balanceList;


            return View(mixmodel);
        }

        public ActionResult CreateChild()
        {
            IEnumerable<PackageViewModel> packageList = (from objPackage in objAceTuitionEntities.tb_package
                                                         where (objPackage.package_status == "active")
                                                         select new PackageViewModel()
                                                         {
                                                             package_id = objPackage.package_id,
                                                             package_english_name = objPackage.package_english_name,
                                                             package_price = objPackage.package_price,
                                                             package_operation = objPackage.package_operation,
                                                             package_chinese_name = objPackage.package_chinese_name,
                                                             package_status = objPackage.package_status,
                                                             package_attribute = objPackage.package_attribute,
                                                             package_subject_amount = objPackage.package_subject_amount

                                                         }).ToList();
            int count = 0;
            var pending = objAceTuitionEntities.tb_payment.Where(x => x.payment_status == "pending").ToList();
            foreach (var p in pending)
            {
                count++;
            }
            TempData["count"] = count;

            return View(packageList);
        }

        public ActionResult AddorEdit(string id)
        {

            var parents = db.tb_parent.Find(id) ?? new tb_parent();
            return PartialView("AddorEdit", parents);
        }

        public ActionResult ParentEdit(string id)
        {

            var parents = db.tb_parent.Find(id) ?? new tb_parent();
            return PartialView("ParentEdit", parents);
        }


        public ActionResult UpdateParent(tb_parent parents)
        {
            
            if (parents.parent_new_password != null)
            {
                if(parents.parent_confirm_password.Length>=6 && parents.parent_new_password.Length >= 6)
                {
                    if (parents.parent_new_password == parents.parent_confirm_password)
                    {
                        parents.parent_password = Hashing.HashPassword(parents.parent_new_password);
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

            if ((string)Session["type"] == "parent")
            {
                Session["name"] = parents.parent_name;
            }
            
            db.Entry(parents).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
            return Json(data: new { success = true, message = "Updated Successfully." }, JsonRequestBehavior.AllowGet);

        }

    

        public ActionResult CreateParent(tb_parent parents)
        {
            //parents.parent_password = parents.parent_ic;
            parents.parent_status = "active";
            parents.parent_password = Hashing.HashPassword(parents.parent_ic);


            var data = db.tb_parent.Where(x => x.parent_ic == parents.parent_ic).SingleOrDefault();
            if (data == null)
            {
                db.tb_parent.Add(parents);
                db.SaveChanges();
                return Json(data: new { success = true, message = "Saved Successfully" }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(data: new { success = false, message = "Parent already created." }, JsonRequestBehavior.AllowGet);

            }

        }
        [HttpPost]
        public ActionResult Delete(ParentsViewModel parent)
        {
            using (var context = new AceTuitionEntities())
            {

                var data = context.tb_parent.FirstOrDefault(x => x.parent_ic == parent.parent_ic);
                if (data != null)
                {
                    context.tb_parent.Remove(data);
                    context.SaveChanges();
                    return Json(data: new { Success = true, Message = "Parent is removed successfully." }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(data: new { Success = false, Message = "" }, JsonRequestBehavior.AllowGet);
                }
            }
        }


    }
}