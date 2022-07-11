using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AceTuitionPaymentSystem.Models;
using AceTuitionPaymentSystem.ViewModel;



namespace AceTuitionPaymentSystem.Controllers
{
    public class LandingController : Controller
    {
        private AceTuitionEntities objAceTuitionEntities;
        AceTuitionEntities db = new AceTuitionEntities();
        public LandingController()
        {
            objAceTuitionEntities = new AceTuitionEntities();
        }
        // GET: Landing
        public ActionResult Index()
        {
            IEnumerable<ChildViewModel> childList = (from objChild in objAceTuitionEntities.tb_child
                                                     where (objChild.child_status == "active")
                                                     select new ChildViewModel()
                                                     {
                                                         child_ic = objChild.child_ic

                                                     }).ToList();

            IEnumerable<SubjectViewModel> subjList = (from objSubject in objAceTuitionEntities.tb_subject
                                                     where (objSubject.subject_status == "active")
                                                     select new SubjectViewModel()
                                                     {
                                                         subject_id = objSubject.subject_id

                                                     }).ToList();

            IEnumerable<PackageViewModel> packList = (from objPackage in objAceTuitionEntities.tb_package
                                                      where (objPackage.package_status == "active")
                                                      select new PackageViewModel()
                                                      {
                                                          package_id = objPackage.package_id,
                                                          package_attribute= objPackage.package_attribute,
                                                          package_english_name=objPackage.package_english_name

                                                      }).ToList();

            var mixmodel = new LandingViewModel();

            mixmodel.childList = childList;
            mixmodel.subjectList = subjList;
            mixmodel.packageList = packList;
            return PartialView(mixmodel);
        }
    }
}