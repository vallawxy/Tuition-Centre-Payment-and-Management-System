using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AceTuitionPaymentSystem.ViewModel
{
    public class AdminListViewModel
    {
        public IEnumerable<ChildViewModel> childList { get; set; }
        public IEnumerable<PaymentViewModel> paymentList { get; set; }
        public IEnumerable<ReceiptViewModel> receiptList { get; set; }
        public IEnumerable<PaymentViewModel> pendingList { get; set; }
        public IEnumerable<PaymentViewModel> totalPaymentList { get; set; }
    }
}