using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AceTuitionPaymentSystem.ViewModel
{
    public class ParentNoticeViewModel
    {
        public IEnumerable<PaymentViewModel> OutstandingList { get; set; }
        public IEnumerable<noticeViewModel> NoticeList { get; set; }
        public IEnumerable<ParentsViewModel> parentList { get; set; }

    }
}