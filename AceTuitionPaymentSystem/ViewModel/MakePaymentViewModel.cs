using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AceTuitionPaymentSystem.ViewModel
{
    public class MakePaymentViewModel
    {
        public IEnumerable<ReceiptViewModel> paymentList { get; set; }

        public IEnumerable<BalanceViewModel> balanceList { get; set; }
        public IEnumerable<BalanceViewModel> accBalance { get; set; }
        public IEnumerable<EarlypaymentViewModel> earlyList { get; set; }
    }
}