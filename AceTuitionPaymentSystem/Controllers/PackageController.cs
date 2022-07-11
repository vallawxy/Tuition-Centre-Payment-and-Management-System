using AceTuitionPaymentSystem.Models;
using AceTuitionPaymentSystem.ViewModel;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace AceTuitionPaymentSystem.Controllers
{
    public class PackageController : Controller
    {
        private AceTuitionEntities objAceTuitionEntities;
        AceTuitionEntities db = new AceTuitionEntities();
        public PackageController()
        {
            objAceTuitionEntities = new AceTuitionEntities();
        }

        // GET: Package
        public ActionResult Index()
        {
            IEnumerable<PackageViewModel> listOfPackageViewModel = (from objPackage in objAceTuitionEntities.tb_package
                                                                    orderby objPackage.package_status, objPackage.package_attribute, objPackage.package_subject_amount, objPackage.package_english_name
                                                                    select new PackageViewModel()
                                                                    {
                                                                        package_id = objPackage.package_id,
                                                                        package_chinese_name = objPackage.package_chinese_name,
                                                                        package_english_name = objPackage.package_english_name,
                                                                        package_status = objPackage.package_status,
                                                                        package_price = objPackage.package_price,
                                                                        package_operation = objPackage.package_operation,
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

            return View(listOfPackageViewModel);
        }


        public ActionResult AddorEdit(int? id)
        {

            var package = db.tb_package.Find(id) ?? new tb_package();
            return PartialView("AddorEdit", package);
        }

        public ActionResult CreateOrUpdate(tb_package package)
        {

            if (package.package_id > 0)
            {

                var ItemPackage = objAceTuitionEntities.tb_package.Where(x => x.package_id == package.package_id).SingleOrDefault();
                if (ItemPackage.package_price == 0)
                {
                        return Json(data: new { success = false, message = "Package Price must greater than 1" }, JsonRequestBehavior.AllowGet);
                 }
                if (ItemPackage.package_english_name == null || ItemPackage.package_chinese_name == null)
                {
                    return Json(data: new { success = false, message = "Package name is required to filled." }, JsonRequestBehavior.AllowGet);
                }
                
                if (ItemPackage.package_attribute == "Primary" || ItemPackage.package_attribute == "Secondary")
                {
                    if (ItemPackage.package_subject_amount == 0 || ItemPackage.package_subject_amount == null)
                    {
                        return Json(data: new { success = false, message = "The numberof subject in this package must greater than 0" }, JsonRequestBehavior.AllowGet);

                    }
                    else
                    {
                        package.package_attribute = ItemPackage.package_attribute;
                        package.package_operation = ItemPackage.package_operation;
                        db.Entry(package).State = System.Data.Entity.EntityState.Modified;
                    }
                    
                }
                else if (ItemPackage.package_attribute == "Childcare")
                {
                    package.package_attribute = ItemPackage.package_attribute;
                    package.package_operation = ItemPackage.package_operation;
                    package.package_subject_amount = 0;
                    db.Entry(package).State = System.Data.Entity.EntityState.Modified;

                }
                else if (ItemPackage.package_attribute == "Meals")
                {
                    package.package_attribute = ItemPackage.package_attribute;
                    package.package_operation = ItemPackage.package_operation;
                    package.package_subject_amount = 0;
                    db.Entry(package).State = System.Data.Entity.EntityState.Modified;

                }
                else if (ItemPackage.package_attribute == "Transport")
                {
                    package.package_attribute = ItemPackage.package_attribute;
                    package.package_operation = ItemPackage.package_operation;
                    package.package_subject_amount = 0;
                    db.Entry(package).State = System.Data.Entity.EntityState.Modified;

                }
                else if (ItemPackage.package_attribute == "Material")
                {
                    package.package_attribute = ItemPackage.package_attribute;
                    package.package_operation = ItemPackage.package_operation;
                    package.package_subject_amount = 0;
                    db.Entry(package).State = System.Data.Entity.EntityState.Modified;

                }

                 db.SaveChanges();
                return Json(data: new { success = true, message = "Updated Successfully." }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                if( package.package_price==0 )
                {
                    return Json(data: new { success = false, message = "Package Price must greater than 1" }, JsonRequestBehavior.AllowGet);
                }
                if(package.package_attribute=="Primary"|| package.package_attribute == "Secondary")
                {
                    if(package.package_english_name==null || package.package_chinese_name == null)
                    {
                        return Json(data: new { success = false, message = "Package name is required to filled." }, JsonRequestBehavior.AllowGet);
                    }
                    if (package.package_subject_amount ==0 || package.package_subject_amount == null)
                    {
                        return Json(data: new { success = false, message = "The numberof subject in this package must greater than 0" }, JsonRequestBehavior.AllowGet);

                    }
                    if (package.package_price == 0)
                    {
                        return Json(data: new { success = false, message = "Package Price must greater than 1" }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                    package.package_operation = "Tuition";
                    package.package_status = "active";
                    db.tb_package.Add(package);
                    }
                    
                   
                }
                else if(package.package_attribute == "Childcare")
                {
                    package.package_chinese_name = "安亲班";
                    package.package_english_name = "Childcare";
                    package.package_operation = "Childcare";
                    package.package_subject_amount = 0;
                    package.package_status = "active";
                    db.tb_package.Add(package);
                   
                }
                else if (package.package_attribute == "Meals")
                {
                    package.package_chinese_name = "伙食";
                    package.package_english_name = "Meals";
                    package.package_operation = "fixed_price";
                    package.package_subject_amount = 0;
                    package.package_status = "active";
                    db.tb_package.Add(package);
                    
                }
                else if (package.package_attribute == "Transport")
                {
                    package.package_operation = "Transport";
                    package.package_chinese_name = "交通";
                    package.package_english_name = "Transportation";
                    package.package_operation = "num_of_transport_day";
                    package.package_status = "active";
                    package.package_subject_amount = 0;
                    db.tb_package.Add(package);

                }
                else if (package.package_attribute == "Material")
                {
                    package.package_chinese_name = "复印费";
                    package.package_english_name = "Material Fee";
                    package.package_operation = "num_of_subject";
                    package.package_status = "active";
                    package.package_subject_amount = 0;
                    db.tb_package.Add(package);
                    
                }
                db.SaveChanges();
                return Json(data: new { success = true, message = "Saved Successfully" }, JsonRequestBehavior.AllowGet);
            }


        }


    }
}