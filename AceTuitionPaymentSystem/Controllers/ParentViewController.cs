using AceTuitionPaymentSystem.Models;
using AceTuitionPaymentSystem.ViewModel;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace AceTuitionPaymentSystem.Controllers
{
    public class ParentViewController : Controller
    {
        // GET: ParentView
        private AceTuitionEntities objAceTuitionEntities;
        AceTuitionEntities db = new AceTuitionEntities();

        public ParentViewController()
        {
            objAceTuitionEntities = new AceTuitionEntities();
        }


        // GET: MakePayment
        public ActionResult Index()
        {
            var parent = Session["ic"];
            IEnumerable<PaymentViewModel> outstandingList = (from objPayment in objAceTuitionEntities.tb_payment
                                                             join objChild in objAceTuitionEntities.tb_child on objPayment.child_ic equals objChild.child_ic
                                                             join objParent in objAceTuitionEntities.tb_parent on objPayment.parent_ic equals objParent.parent_ic
                                                             where( (objPayment.payment_status == "outstanding" || objPayment.payment_status == "pending" )&& objParent.parent_ic == parent.ToString())
                                                             select new PaymentViewModel()
                                                             {
                                                                 payment_id = objPayment.payment_id,
                                                                 payment_date = objPayment.payment_date,
                                                                 payment_upload_date = objPayment.payment_upload_date,
                                                                 payment_status = objPayment.payment_status,
                                                                 payment_amount = objPayment.payment_amount,
                                                                 payment_month = objPayment.payment_month,
                                                                 payment_year = objPayment.payment_year,
                                                                 payment_outstanding = objPayment.payment_outstanding,
                                                                 parent_ic = objPayment.parent_ic,
                                                                 child_ic = objPayment.child_ic,
                                                                 child_name_eng = objChild.child_name_eng,
                                                                 child_code = objChild.child_code,
                                                                 child_name_chinese = objChild.child_name_chinese,
                                                                 parent_name = objParent.parent_name,
                                                                 parent_phone = objParent.parent_phone,
       
                                                             }).ToList();

            IEnumerable<noticeViewModel> listOfnotice = (from objNotice in objAceTuitionEntities.tb_notice
                                                                  select new noticeViewModel()
                                                                  {
                                                                      notice_id = objNotice.notice_id,
                                                                      notice_date = objNotice.notice_date,
                                                                      title = objNotice.title,
                                                                      description = objNotice.description,

                                                                  }).ToList();
            IEnumerable<ParentsViewModel> parentList = (from objParent in objAceTuitionEntities.tb_parent
                                                        where(objParent.parent_ic== parent.ToString())
                                                         select new ParentsViewModel()
                                                         {
                                                            parent_address=objParent.parent_address,
                                                             parent_ic = objParent.parent_ic,
                                                             parent_name = objParent.parent_name,
                                                             parent_phone = objParent.parent_phone,
                                                            
                                                         }).ToList();

            var mix = new ParentNoticeViewModel();

            mix.OutstandingList = outstandingList;
            mix.NoticeList = listOfnotice;
            mix.parentList = parentList;


            return View(mix);

        }
    }
}