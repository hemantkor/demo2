using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SessionManagement.Models
{
    public class UserLogin
    {
        [DataType(DataType.Text)]
        [Required(ErrorMessage = "{0} is required")]
        public string LoginName { get; set; }

        [DataType(DataType.Text)]
        [Required(ErrorMessage = "{0} is required")]
        public string Password { get; set; }

        [Display(Name = " Remember me")]
        public Boolean Remember { get; set; }
    }
}