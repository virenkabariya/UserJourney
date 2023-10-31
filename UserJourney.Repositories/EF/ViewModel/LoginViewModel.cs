using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using UserJourney.Core.Constants;

namespace UserJourney.Repositories.EF.ViewModel
{
    public class LoginViewModel
    {
        [StringLength(100)]
        [Display(Name = "Email")]
        [Required]
        [RegularExpression(RegularExpressions.Email, ErrorMessage = SystemMessage.EmailErrorMessage)]
        public string Email { get; set; }

        [StringLength(50)]
        [Required]
        [DataType(DataType.Password)]
        [RegularExpression(RegularExpressions.HtmlTag, ErrorMessage = SystemMessage.InvalidInput)]
        public string Password { get; set; }

        public bool RememberMe { get; set; }
    }
}
