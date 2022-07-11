using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AceTuitionPaymentSystem.ViewModel
{
    public class LandingViewModel
    {
        public IEnumerable<ChildViewModel> childList;
        public IEnumerable<SubjectViewModel> subjectList;
        public IEnumerable<PackageViewModel> packageList;
       
    }
}