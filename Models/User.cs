using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SessionManagement.Models
{
    public class User
    {
        [Key]
        [DataType(DataType.Text)]
        [Required(ErrorMessage = "Please enter Login name")]
        [StringLength(20, MinimumLength = 4, ErrorMessage = "Login name must be greater than 4 Characters")]
        public String LoginName { get; set; }

        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Please enter Password")]
        [StringLength(20, MinimumLength = 4, ErrorMessage = "{0} must be greater than {2} Characters")]
        public String Password { get; set; }

        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Password didn't match")]
        public String ConfirmPassword { get; set; }

        [DataType(DataType.Text)]
        [Required(ErrorMessage = "{0} is Required")]
        public String FullName { get; set; }

        [DataType(DataType.Text)]
        [Required(ErrorMessage = "{0} is Required")]
        public String EmailId { get; set; }

        [Required(ErrorMessage = "{0} is Required")]
        public String City { get; set; }

        [DataType(DataType.PhoneNumber)]
        [Required(ErrorMessage = "{0} is Required")]
        [StringLength(10, ErrorMessage = "{0} should be of {1} Numbers", MinimumLength = 10)]
        public String Phone { get; set; }
    }
}