using AceTuitionPaymentSystem.Models;
using AceTuitionPaymentSystem.ViewModel;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AceTuitionPaymentSystem.Controllers
{
    public class ChildController : Controller
    {

        private AceTuitionEntities objAceTuitionEntities;
        
        public ChildController()
        {
            objAceTuitionEntities = new AceTuitionEntities();
        }

        // GET: Child
        public ActionResult Index(string parent, string status)
        {
            string selectedStatus = "";
            if (status =="active")
            {
                selectedStatus = "active";
            } else if (status == "inactive")
            {
                selectedStatus = "inactive";
            }

            IEnumerable<ChildViewModel> listOfChildViewModel = null;

            if (status == "all")
            {
                if (parent != null)
                {

                    listOfChildViewModel = (from objChild in objAceTuitionEntities.tb_child
                                            where (objChild.child_parent_ic == parent)
                                            orderby objChild.child_status, objChild.child_code
                                            select new ChildViewModel()
                                            {

                                                child_ic = objChild.child_ic,
                                                child_name_eng = objChild.child_name_eng,
                                                child_name_chinese = objChild.child_name_chinese,
                                                child_parent_ic = objChild.child_parent_ic,
                                                child_code = objChild.child_code,
                                                child_school = objChild.child_school,
                                                child_year = objChild.child_year,
                                                child_reg_date = objChild.child_reg_date,
                                                child_trans_day = objChild.child_trans_day,
                                                child_num_subject = objChild.child_num_subject,
                                                child_status = objChild.child_status,

                                            }).ToList();

                }
                else
                {
                    listOfChildViewModel = (from objChild in objAceTuitionEntities.tb_child
                                            orderby objChild.child_status, objChild.child_code
                                            select new ChildViewModel()
                                            {

                                                child_ic = objChild.child_ic,
                                                child_name_eng = objChild.child_name_eng,
                                                child_name_chinese = objChild.child_name_chinese,
                                                child_parent_ic = objChild.child_parent_ic,
                                                child_code = objChild.child_code,
                                                child_school = objChild.child_school,
                                                child_year = objChild.child_year,
                                                child_reg_date = objChild.child_reg_date,
                                                child_trans_day = objChild.child_trans_day,
                                                child_num_subject = objChild.child_num_subject,
                                                child_status = objChild.child_status,

                                            }).ToList();

                }
            } else
            {
                if (parent != null && status != null)
                {

                    listOfChildViewModel = (from objChild in objAceTuitionEntities.tb_child
                                            where (objChild.child_parent_ic == parent && objChild.child_status == selectedStatus)
                                            orderby objChild.child_status, objChild.child_code
                                            select new ChildViewModel()
                                            {

                                                child_ic = objChild.child_ic,
                                                child_name_eng = objChild.child_name_eng,
                                                child_name_chinese = objChild.child_name_chinese,
                                                child_parent_ic = objChild.child_parent_ic,
                                                child_code = objChild.child_code,
                                                child_school = objChild.child_school,
                                                child_year = objChild.child_year,
                                                child_reg_date = objChild.child_reg_date,
                                                child_trans_day = objChild.child_trans_day,
                                                child_num_subject = objChild.child_num_subject,
                                                child_status = objChild.child_status,

                                            }).ToList();

                }
                else if (status != null && parent == null)
                {
                    listOfChildViewModel = (from objChild in objAceTuitionEntities.tb_child
                                            where (objChild.child_status == selectedStatus)
                                            orderby objChild.child_status, objChild.child_code
                                            select new ChildViewModel()
                                            {

                                                child_ic = objChild.child_ic,
                                                child_name_eng = objChild.child_name_eng,
                                                child_name_chinese = objChild.child_name_chinese,
                                                child_parent_ic = objChild.child_parent_ic,
                                                child_code = objChild.child_code,
                                                child_school = objChild.child_school,
                                                child_year = objChild.child_year,
                                                child_reg_date = objChild.child_reg_date,
                                                child_trans_day = objChild.child_trans_day,
                                                child_num_subject = objChild.child_num_subject,
                                                child_status = objChild.child_status,

                                            }).ToList();

                } else if (status == null && parent != null)
                {
                    listOfChildViewModel = (from objChild in objAceTuitionEntities.tb_child
                                            where (objChild.child_parent_ic == parent)
                                            orderby objChild.child_status, objChild.child_code
                                            select new ChildViewModel()
                                            {

                                                child_ic = objChild.child_ic,
                                                child_name_eng = objChild.child_name_eng,
                                                child_name_chinese = objChild.child_name_chinese,
                                                child_parent_ic = objChild.child_parent_ic,
                                                child_code = objChild.child_code,
                                                child_school = objChild.child_school,
                                                child_year = objChild.child_year,
                                                child_reg_date = objChild.child_reg_date,
                                                child_trans_day = objChild.child_trans_day,
                                                child_num_subject = objChild.child_num_subject,
                                                child_status = objChild.child_status,

                                            }).ToList();
                } else {
                    listOfChildViewModel = (from objChild in objAceTuitionEntities.tb_child
                     orderby objChild.child_status, objChild.child_code
                     select new ChildViewModel()
                     {

                         child_ic = objChild.child_ic,
                         child_name_eng = objChild.child_name_eng,
                         child_name_chinese = objChild.child_name_chinese,
                         child_parent_ic = objChild.child_parent_ic,
                         child_code = objChild.child_code,
                         child_school = objChild.child_school,
                         child_year = objChild.child_year,
                         child_reg_date = objChild.child_reg_date,
                         child_trans_day = objChild.child_trans_day,
                         child_num_subject = objChild.child_num_subject,
                         child_status = objChild.child_status,

                     }).ToList(); ;
                }
            }

            






            int month = DateTime.Now.Month;
            int year = DateTime.Now.Year;

            IEnumerable<PaymentViewModel> listofChildYesPayment = (from objPayment in objAceTuitionEntities.tb_payment
                                                                  where(objPayment.payment_month==month && objPayment.payment_year==year)
                                                                select new PaymentViewModel()
                                                                {

                                                                    child_ic = objPayment.child_ic,
                                                                    parent_ic = objPayment.parent_ic,
                                                                    payment_year = objPayment.payment_year,
                                                                    payment_month = objPayment.payment_month,
                                                                    payment_id = objPayment.payment_id,
                                                                   
                                                                }).ToList();

            int count = 0;
            var pending = objAceTuitionEntities.tb_payment.Where(x => x.payment_status == "pending").ToList();
            foreach (var p in pending)
            {
                count++;
            }
            TempData["count"] = count;
            var mixmodel = new Student_PaymentViewModel();

            mixmodel.childList = listOfChildViewModel;
            mixmodel.childyesPaymentList = listofChildYesPayment;
            
            return View(mixmodel);
        }

        public ActionResult InactiveStudent()
        {
            IEnumerable<ChildViewModel> inactiveChild = (from objChild in objAceTuitionEntities.tb_child
                                                         where (objChild.child_status == "inactive")
                                                         orderby objChild.child_status, objChild.child_code
                                                         select new ChildViewModel()
                                                         {

                                                             child_ic = objChild.child_ic,
                                                             child_name_eng = objChild.child_name_eng,
                                                             child_name_chinese = objChild.child_name_chinese,
                                                             child_parent_ic = objChild.child_parent_ic,
                                                             child_code = objChild.child_code,
                                                             child_school = objChild.child_school,
                                                             child_year = objChild.child_year,
                                                             child_reg_date = objChild.child_reg_date,
                                                             child_trans_day = objChild.child_trans_day,
                                                             child_num_subject = objChild.child_num_subject,
                                                             child_status = objChild.child_status,

                                                         }).ToList();



            int count = 0;
            var pending = objAceTuitionEntities.tb_payment.Where(x => x.payment_status == "pending").ToList();
            foreach (var p in pending)
            {
                count++;
            }
            TempData["count"] = count;

            return View(inactiveChild);
        }
        public ActionResult EditStatus()
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

        //public ActionResult UpdateStatus(tb_child child)
        //{

        //    objAceTuitionEntities.Entry(child).State = System.Data.Entity.EntityState.Modified;
        //    objAceTuitionEntities.SaveChanges();
        //    return Json(data: new { success = true, message = "Updated Successfully" }, JsonRequestBehavior.AllowGet);
            
           
        //}




        public ActionResult CreateChild()
        {
            return View();
        }

        public ActionResult EditChild()
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

        [HttpPost]
        public JsonResult FetchEditSubject(ChildViewModel objChildViewModel)
        {
            using (var context = new AceTuitionEntities())
            {

                Console.WriteLine(objChildViewModel.child_ic);

                context.Configuration.AutoDetectChangesEnabled = false; //added line
                context.Configuration.LazyLoadingEnabled = false; //added line
                context.Configuration.ProxyCreationEnabled = false; //added line


                var data = context.tb_child.Where(x => x.child_ic == objChildViewModel.child_ic).SingleOrDefault();

                var subjectTaken = context.tb_child_subject.Where(x => x.child_ic == objChildViewModel.child_ic).ToList();
               
                if (subjectTaken!=null)
                {
                    var subjectList = context.tb_subject.Where(x => x.subject_grade == data.child_year && x.subject_status == "active").ToList();
                    foreach (var subject in subjectTaken)
                    {
                        subject.tb_child = null;
                        subject.tb_subject = null;
                    }
                    return Json(data: new { Success = true, SubjectTaken = subjectTaken, SubjectList = subjectList }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    var subjectlist = context.tb_subject.Where(x => x.subject_grade == data.child_year && x.subject_status == "active").ToList();
                    foreach (var subject in subjectTaken)
                    {
                        subject.tb_child = null;
                        subject.tb_subject = null;
                    }
                    return Json(data: new { Success = true, SubjectTaken = subjectTaken, SubjectList = subjectlist }, JsonRequestBehavior.AllowGet);
                }

                


            }
        }




        public JsonResult FetchEditPackage(ChildViewModel objChildViewModel)
        {
            using (var context = new AceTuitionEntities())
            {

                Console.WriteLine(objChildViewModel.child_ic);

                context.Configuration.AutoDetectChangesEnabled = false; //added line
                context.Configuration.LazyLoadingEnabled = false; //added line
                context.Configuration.ProxyCreationEnabled = false; //added line

                var packageTaken = context.tb_child_package.Where(x => x.child_ic == objChildViewModel.child_ic).ToList();

                var child= context.tb_child.Where(x => x.child_ic == objChildViewModel.child_ic).FirstOrDefault();
                var grade = int.Parse(child.child_year);

                List<tb_package> packageList;
                packageList = context.tb_package.Where(x => x.package_status == "active").ToList();

                

                //if (grade < 7)
                //{
                //    context.Configuration.LazyLoadingEnabled = false;
                    
                    
                //}
                //else
                //{
                //    context.Configuration.LazyLoadingEnabled = false;
                //    packageList = context.tb_package.Where(x => x.package_status == "active" && (x.package_attribute == "Transport" || x.package_attribute == "Secondary" || x.package_attribute == "Material")).ToList();
                    
                //}

                foreach (var package in packageTaken)
                {
                    package.tb_child = null;
                    package.tb_package = null;
                }

                return Json(data: new { Success = true, PackageTaken = packageTaken, PackageList = packageList }, JsonRequestBehavior.AllowGet);
            }
        }






        public JsonResult FetchEditDiscount(ChildViewModel objChildViewModel)
        {
            using (var context = new AceTuitionEntities())
            {

                Console.WriteLine(objChildViewModel.child_ic);

                context.Configuration.AutoDetectChangesEnabled = false; //added line
                context.Configuration.LazyLoadingEnabled = false; //added line
                context.Configuration.ProxyCreationEnabled = false; //added line

                var discountList = context.tb_discount.Where(x => x.child_ic == objChildViewModel.child_ic && x.discount_status == "active").ToList();

                return Json(data: new { Success = true, DiscountList = discountList }, JsonRequestBehavior.AllowGet);
            }
        }






        public JsonResult FetchEditAddon(ChildViewModel objChildViewModel)
        {
            using (var context = new AceTuitionEntities())
            {

                Console.WriteLine(objChildViewModel.child_ic);

                context.Configuration.AutoDetectChangesEnabled = false; //added line
                context.Configuration.LazyLoadingEnabled = false; //added line
                context.Configuration.ProxyCreationEnabled = false; //added line

                var addonList = context.tb_addon.Where(x => x.child_ic == objChildViewModel.child_ic && x.addon_status == "active").ToList();

                return Json(data: new { Success = true, AddonList = addonList }, JsonRequestBehavior.AllowGet);
            }
        }



        [HttpPost]
        public JsonResult FetchEditChildren(ChildViewModel objChildViewModel)
        {
            using (var context = new AceTuitionEntities())
            {

                Console.WriteLine(objChildViewModel.child_ic);

                context.Configuration.AutoDetectChangesEnabled = false; //added line
                context.Configuration.LazyLoadingEnabled = false; //added line
                context.Configuration.ProxyCreationEnabled = false; //added line


                var data = context.tb_child.Where(x => x.child_ic == objChildViewModel.child_ic).SingleOrDefault();


                return Json(data: new { Success = true, Data = data }, JsonRequestBehavior.AllowGet);
            }
        }




        [HttpPost]
        public ActionResult Edit(ChildViewModel objChildViewModel)
        {
            using (var context = new AceTuitionEntities())
            {



                var data = context.tb_child.Where(x => x.child_ic == objChildViewModel.child_ic).SingleOrDefault();
                if (data != null)
                {


                    if (objChildViewModel.child_package_taken != null)
                    {
                        string[] packageTakenList = objChildViewModel.child_package_taken.Split(",".ToCharArray());
                        for (var i = 0; i < packageTakenList.Length; i++)
                        {
                            var id = Int16.Parse(packageTakenList[i]);
                            var pack = context.tb_child_package.FirstOrDefault(x => x.child_ic == objChildViewModel.child_ic && x.package_id == id);
                            context.tb_child_package.Remove(pack);
                        }

                    }

                    if (objChildViewModel.child_subject_taken != null)
                    {
                        string[] subjectTakenList = objChildViewModel.child_subject_taken.Split(",".ToCharArray());
                        for (var i = 0; i < subjectTakenList.Length; i++)
                        {
                            var id = Int16.Parse(subjectTakenList[i]);
                            var sub = context.tb_child_subject.FirstOrDefault(x => x.child_ic == objChildViewModel.child_ic && x.subject_id == id);
                            context.tb_child_subject.Remove(sub);
                        }

                    }



                    if (objChildViewModel.child_addon_taken != null)
                    {
                        string[] addonTakenList = objChildViewModel.child_addon_taken.Split(",".ToCharArray());
                        for (var i = 0; i < addonTakenList.Length; i++)
                        {
                            var id = Int16.Parse(addonTakenList[i]);
                            var addon = context.tb_addon.FirstOrDefault(x => x.child_ic == objChildViewModel.child_ic && x.addon_id == id);
                            if (addon != null)
                            {
                                addon.addon_status = "inactive";
                            }

                        }
                    }


                    if (objChildViewModel.child_discount_taken != null)
                    {
                        string[] discountTakenList = objChildViewModel.child_discount_taken.Split(",".ToCharArray());
                        for (var i = 0; i < discountTakenList.Length; i++)
                        {
                            var id = Int16.Parse(discountTakenList[i]);
                            var discount = context.tb_discount.FirstOrDefault(x => x.child_ic == objChildViewModel.child_ic && x.discount_id == id);
                            if (discount != null)
                            {
                                discount.discount_status = "inactive";
                            }

                        }
                    }




                    if (objChildViewModel.child_subject_list != null)
                    {
                        string[] subjectList = objChildViewModel.child_subject_list.Split(",".ToCharArray());
                        for (var i = 0; i < subjectList.Length; i++)
                        {
                            tb_child_subject obj = new tb_child_subject();
                            obj.subject_id = Int16.Parse(subjectList[i]);
                            obj.child_ic = objChildViewModel.child_ic;
                            objAceTuitionEntities.tb_child_subject.Add(obj);
                        }

                        data.child_num_subject = subjectList.Length;
                    }
                    else
                    {
                        data.child_num_subject = 0;
                    }


                    if (objChildViewModel.child_package_list != null)
                    {
                        string[] packageList = objChildViewModel.child_package_list.Split(",".ToCharArray());
                        for (var i = 0; i < packageList.Length; i++)
                        {
                            tb_child_package pobj = new tb_child_package();
                            pobj.package_id = Int16.Parse(packageList[i]);
                            pobj.child_ic = objChildViewModel.child_ic;
                            objAceTuitionEntities.tb_child_package.Add(pobj);
                        }
                    }

                    if (objChildViewModel.child_addon_english_name_list != null)
                    {
                        string[] addonEnglishNameList = objChildViewModel.child_addon_english_name_list.Split(",".ToCharArray());
                        string[] addonChineseNameList = objChildViewModel.child_addon_chinese_name_list.Split(",".ToCharArray());
                        string[] addonValueList = objChildViewModel.child_addon_value_list.Split(",".ToCharArray());
                        string[] addonRecursiveList = objChildViewModel.child_addon_recursive_list.Split(",".ToCharArray());

                        for (var i = 0; i < addonEnglishNameList.Length; i++)
                        {
                            tb_addon addonObj = new tb_addon();
                            addonObj.addon_english_name = addonEnglishNameList[i];
                            addonObj.addon_chinese_name = addonChineseNameList[i];
                            addonObj.addon_value = decimal.Parse(addonValueList[i]);
                            addonObj.addon_status = "active";
                            addonObj.child_ic = objChildViewModel.child_ic;
                            addonObj.addon_recursive = addonRecursiveList[i];
                            objAceTuitionEntities.tb_addon.Add(addonObj);
                        }

                    }


                    if (objChildViewModel.child_discount_english_name_list != null)
                    {
                        string[] discountEnglishNameList = objChildViewModel.child_discount_english_name_list.Split(",".ToCharArray());
                        string[] discountChineseNameList = objChildViewModel.child_discount_chinese_name_list.Split(",".ToCharArray());
                        string[] discountValueList = objChildViewModel.child_discount_value_list.Split(",".ToCharArray());
                        string[] discountRecursiveList = objChildViewModel.child_discount_recursive_list.Split(",".ToCharArray());



                        for (var i = 0; i < discountEnglishNameList.Length; i++)
                        {
                            tb_discount discountObj = new tb_discount();
                            discountObj.discount_english_name = discountEnglishNameList[i];
                            discountObj.discount_chinese_name = discountChineseNameList[i];
                            discountObj.discount_value = decimal.Parse(discountValueList[i]);
                            discountObj.discount_status = "active";
                            discountObj.child_ic = objChildViewModel.child_ic;
                            discountObj.discount_recursive = discountRecursiveList[i];
                            objAceTuitionEntities.tb_discount.Add(discountObj);
                        }
                    }

                    var count = context.tb_child.Count(t => t.child_year == objChildViewModel.child_year);
                    string[] gradeArray = { "A", "B", "C", "D", "E", "F", "1", "2", "3", "4", "5","PER" };
                    data.child_code = "ACE " + gradeArray[Int16.Parse(objChildViewModel.child_year) - 1] + count.ToString().PadLeft(4, '0');


                    data.child_name_eng = objChildViewModel.child_name_eng;
                    data.child_name_chinese = objChildViewModel.child_name_chinese;
                    data.child_school = objChildViewModel.child_school;
                    data.child_year = objChildViewModel.child_year;
                    data.child_trans_day = objChildViewModel.child_trans_day;

                    data.child_status = objChildViewModel.child_status;

                    context.SaveChanges();
                    objAceTuitionEntities.SaveChanges();

                    return Json(data: new { Success = true, Message = "Child is updated successfully." }, JsonRequestBehavior.AllowGet);

                }

                return Json(data: new { Success = false, Message = "" }, JsonRequestBehavior.AllowGet);
            }
        }



        [HttpPost]
        public ActionResult Delete(ChildViewModel objChildViewModel)
        {
            using (var context = new AceTuitionEntities())
            {

                var data = context.tb_child.FirstOrDefault(x => x.child_ic == objChildViewModel.child_ic);
                if (data != null)
                {
                   context.tb_child.Remove(data);
                    context.SaveChanges();
                    return Json(data: new { Success = true, Message = "Child is removed successfully." }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(data: new { Success = false, Message = "" }, JsonRequestBehavior.AllowGet);
                }
            }
        }

        [HttpPost]
        public ActionResult Create(ChildViewModel objChildViewModel)
        {

            using (var context = new AceTuitionEntities())
            {

                tb_child objChild = new tb_child();

                objChild.child_ic = objChildViewModel.child_ic;

                var count = context.tb_child.Count(t => t.child_year == objChildViewModel.child_year) + 1;
                string[] gradeArray = { "A", "B", "C", "D", "E", "F", "1", "2", "3", "4", "5","PER" };
                objChild.child_code = "ACE " + gradeArray[Int16.Parse(objChildViewModel.child_year) - 1] + count.ToString().PadLeft(4, '0');
                objChild.child_reg_date = objChildViewModel.child_reg_date;
                objChild.child_name_eng = objChildViewModel.child_name_eng;
                objChild.child_name_chinese = objChildViewModel.child_name_chinese;
                objChild.child_parent_ic = objChildViewModel.child_parent_ic;
                objChild.child_school = objChildViewModel.child_school;
                objChild.child_year = objChildViewModel.child_year;
                objChild.child_trans_day = objChildViewModel.child_trans_day;

                if (objChildViewModel.child_subject_list != null)
                {
                    string[] subjectList = objChildViewModel.child_subject_list.Split(",".ToCharArray());
                    for (var i = 0; i < subjectList.Length; i++)
                    {
                        tb_child_subject obj = new tb_child_subject();
                        obj.subject_id = Int16.Parse(subjectList[i]);
                        obj.child_ic = objChildViewModel.child_ic;
                        objAceTuitionEntities.tb_child_subject.Add(obj);
                    }

                    objChild.child_num_subject = subjectList.Length;
                }
                else
                {
                    objChild.child_num_subject = 0;
                }

                objChild.child_status = "active";
                objAceTuitionEntities.tb_child.Add(objChild);

                if (objChildViewModel.child_package_list != null)
                {
                    string[] packageList = objChildViewModel.child_package_list.Split(",".ToCharArray());

                    for (var i = 0; i < packageList.Length; i++)
                    {
                        tb_child_package pobj = new tb_child_package();
                        pobj.package_id = Int16.Parse(packageList[i]);
                        pobj.child_ic = objChildViewModel.child_ic;
                        objAceTuitionEntities.tb_child_package.Add(pobj);
                    }
                }





                if (objChildViewModel.child_addon_english_name_list != null)
                {
                    string[] addonEnglishNameList = objChildViewModel.child_addon_english_name_list.Split(",".ToCharArray());
                    string[] addonChineseNameList = objChildViewModel.child_addon_chinese_name_list.Split(",".ToCharArray());
                    string[] addonValueList = objChildViewModel.child_addon_value_list.Split(",".ToCharArray());
                    string[] addonRecursiveList = objChildViewModel.child_addon_recursive_list.Split(",".ToCharArray());

                    for (var i = 0; i < addonEnglishNameList.Length; i++)
                    {
                        tb_addon addonObj = new tb_addon();
                        addonObj.addon_english_name = addonEnglishNameList[i];
                        addonObj.addon_chinese_name = addonChineseNameList[i];
                        addonObj.addon_value = decimal.Parse(addonValueList[i]);
                        addonObj.addon_status = "active";
                        addonObj.child_ic = objChildViewModel.child_ic;
                        addonObj.addon_recursive = addonRecursiveList[i];
                        objAceTuitionEntities.tb_addon.Add(addonObj);
                    }

                }


                if (objChildViewModel.child_discount_english_name_list != null)
                {
                    string[] discountEnglishNameList = objChildViewModel.child_discount_english_name_list.Split(",".ToCharArray());
                    string[] discountChineseNameList = objChildViewModel.child_discount_chinese_name_list.Split(",".ToCharArray());
                    string[] discountValueList = objChildViewModel.child_discount_value_list.Split(",".ToCharArray());
                    string[] discountRecursiveList = objChildViewModel.child_discount_recursive_list.Split(",".ToCharArray());



                    for (var i = 0; i < discountEnglishNameList.Length; i++)
                    {
                        tb_discount discountObj = new tb_discount();
                        discountObj.discount_english_name = discountEnglishNameList[i];
                        discountObj.discount_chinese_name = discountChineseNameList[i];
                        discountObj.discount_value = decimal.Parse(discountValueList[i]);
                        discountObj.discount_status = "active";
                        discountObj.child_ic = objChildViewModel.child_ic;
                        discountObj.discount_recursive = discountRecursiveList[i];
                        objAceTuitionEntities.tb_discount.Add(discountObj);
                    }
                }


               objAceTuitionEntities.SaveChanges();
                return Json(data: new { Success = true, Message = "Child is added successfully." }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult FetchSubject(string grade)
        {
            using (var context = new AceTuitionEntities())
            {
                context.Configuration.LazyLoadingEnabled = false;
                var data = context.tb_subject.Where(x => x.subject_grade == grade && x.subject_status == "active").ToList();
                return Json(data: new { Success = true, Data = data }, JsonRequestBehavior.AllowGet);
            }
        }


        [HttpPost]
        public JsonResult FetchPackage(string grade)
        {
            using (var context = new AceTuitionEntities())
            {
                var stuY = int.Parse(grade);
                if(stuY < 7)
                {
                        context.Configuration.LazyLoadingEnabled = false;
                        var data = context.tb_package.Where(x => x.package_status == "active").ToList();
                        return Json(data: new { Success = true, Data = data }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    context.Configuration.LazyLoadingEnabled = false;
                    var data = context.tb_package.Where(x => x.package_status == "active").ToList();
                    //var data = context.tb_package.Where(x => x.package_status == "active"&&(x.package_attribute=="Transport" || x.package_attribute == "Secondary" || x.package_attribute == "Material")).ToList();
                    return Json(data: new { Success = true, Data = data }, JsonRequestBehavior.AllowGet);
                }
                    
                
               
            }
        }

    }
}
