using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AceTuitionPaymentSystem.ViewModel
{
    public class ParentBalanceViewModel
    {
        public IEnumerable<ParentsViewModel> parentList { get; set; }
        public IEnumerable<BalanceViewModel> balanceList { get; set; }
    }
}