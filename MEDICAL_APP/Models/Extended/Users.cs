using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace MEDICAL_APP.Models
{ 
    [MetadataType(typeof(UserMetaData))]
    public partial class Users
    {
        public string ConfirmPassword { get; set; }

    }

    public class UserMetaData
    {
        [Display(Name = "First Name")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "field required")]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "field required")]
        public string LastName { get; set; }

        [Display(Name = "Email Id")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "field required")]
        [DataType(DataType.EmailAddress)]
        public string EmailId { get; set; }

        [Display(Name = "Date of birth")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM-dd-yyyy}")]
        public string DateOfBirth { get; set; }

        
        [Required(AllowEmptyStrings = false, ErrorMessage = "field required")]
        [DataType(DataType.Password)]
        [MinLength(6,ErrorMessage ="Minimum is 6 chars")]
        public string Password { get; set; }

        [Display(Name ="Confirm Password")]
        [DataType(DataType.Password)]
        [Compare("Password",ErrorMessage ="Passwords need be same")]
        public string ConfirmPassword { get; set; }
    }
}