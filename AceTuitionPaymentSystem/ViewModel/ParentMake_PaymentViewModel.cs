using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AceTuitionPaymentSystem.ViewModel
{
    public class ParentMake_PaymentViewModel
    {
        public IEnumerable<PaymentViewModel> paymentList { get; set; }
        public IEnumerable<BalanceViewModel> balanceList { get; set; }
        public IEnumerable<BalanceViewModel> accBalance { get; set; }
        public IEnumerable<EarlypaymentViewModel> earlyList { get; set; }

    }
}