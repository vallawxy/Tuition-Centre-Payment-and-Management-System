using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AceTuitionPaymentSystem.Models;
using AceTuitionPaymentSystem.ViewModel;


namespace AceTuitionPaymentSystem.Controllers
{
    public class ParentView_ChildController : Controller
    {
        private AceTuitionEntities objAceTuitionEntities = new AceTuitionEntities();


        // GET: ParentView_Child
        public ActionResult Index()
        {
            var parent_ic = Session["ic"];
            IEnumerable<ParentView_Child> listOfChildViewModel = (from objChild in objAceTuitionEntities.tb_child
                                                                  where (objChild.child_parent_ic == parent_ic.ToString() && objChild.child_status == "active")
                                                                  let childSubject = from objSubject in objAceTuitionEntities.tb_child_subject
                                                                                               where (objSubject.child_ic == objChild.child_ic)
                                                                                               select new ChildSubjectViewModel()
                                                                                               {
                                                                                                   child_ic = objSubject.child_ic,
                                                                                                   subject_id = objSubject.subject_id,
                                                                                                   subject_chinese_name = objSubject.tb_subject.subject_chinese_name,
                                                                                                   subject_english_name = objSubject.tb_subject.subject_english_name,
                                                                                                   subject_grade = objSubject.tb_subject.subject_grade,
                                                                                                   subject_status = objSubject.tb_subject.subject_status

                                                                                               }
                                                                    let childPackage = from objPackage in objAceTuitionEntities.tb_child_package
                                                                                       where (objPackage.child_ic == objChild.child_ic)
                                                                                       select new ChildPackageViewModel()
                                                                                       {
                                                                                           child_ic = objPackage.child_ic,
                                                                                           package_id = objPackage.package_id,
                                                                                           package_chinese_name = objPackage.tb_package.package_chinese_name,
                                                                                           package_english_name = objPackage.tb_package.package_english_name,
                                                                                           package_operation = objPackage.tb_package.package_operation,
                                                                                           package_status = objPackage.tb_package.package_status,
                                                                                           package_price = objPackage.tb_package.package_price
                                                                                       }

                                                                  select new ParentView_Child()
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

                                                                      child_childsubject_list = childSubject,
                                                                      child_childpackage_list = childPackage
                                                                     
                                                                  }).ToList();
            return View(listOfChildViewModel);

        }
    }
}