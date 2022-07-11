using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AceTuitionPaymentSystem.ViewModel
{
    public class Student_PaymentViewModel
    {
        public IEnumerable<ChildViewModel> childList { get; set; }
        public IEnumerable<PaymentViewModel> childyesPaymentList { get; set; }
    }
}