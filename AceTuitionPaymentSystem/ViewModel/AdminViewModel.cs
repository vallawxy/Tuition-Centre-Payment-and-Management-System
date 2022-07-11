using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace AceTuitionPaymentSystem.ViewModel
{
    public class AdminViewModel
    {

        [Required(ErrorMessage = "This Field is Required")]
        public string admin_ic { set; get; }
        public string admin_name { get; set; }
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "This Field is Required")]
        public string admin_password { get; set; }
        public string LoginErrorMessage { get; set; }
        public IEnumerable<SelectListItem> AdminSelectListItem { get; set; }

    }
}