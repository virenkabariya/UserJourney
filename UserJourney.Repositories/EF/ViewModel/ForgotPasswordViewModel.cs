using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using UserJourney.Core.Constants;

namespace UserJourney.Repositories.EF.ViewModel
{
    public class ForgotPasswordViewModel
    {
        [StringLength(100)]
        [Display(Name = "Email")]
        [Required]
        [RegularExpression(RegularExpressions.Email, ErrorMessage = SystemMessage.EmailErrorMessage)]
        public string Email { get; set; }

    }
}
