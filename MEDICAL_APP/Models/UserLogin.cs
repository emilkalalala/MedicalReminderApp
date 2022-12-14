using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MEDICAL_APP.Models
{
    public class UserLogin
    {
        [Display( Name="EmailId")]
        [Required(AllowEmptyStrings =false, ErrorMessage ="required field")]
        public string EmailId { get; set; }

        [Display(Name = "Password")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "required field")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Display(Name = "Remember me")]
        public bool RememberMe { get; set; }
    
}
}