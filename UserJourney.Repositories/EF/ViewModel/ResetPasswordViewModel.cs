using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using UserJourney.Core.Constants;

namespace UserJourney.Repositories.EF.ViewModel
{
    public class ResetPasswordViewModel
    {
        public int UserId { get; set; }

        [Required]
        [StringLength(50)]
        [DataType(DataType.Password)]
        [Display(Name = "Old Password")]
        [RegularExpression(RegularExpressions.Password, ErrorMessage = SystemMessage.InvalidPassword)]
        public string OldPassword { get; set; }

        [Required]
        [StringLength(50)]
        [DataType(DataType.Password)]
        [Display(Name = "New Password")]
        [RegularExpression(RegularExpressions.Password, ErrorMessage = SystemMessage.InvalidPassword)]
        public string Password { get; set; }

        [NotMapped]
        [StringLength(50)]
        [DataType(DataType.Password)]
        [Compare("Password")]
        [Display(Name = "Confirm New Password")]
        [RegularExpression(RegularExpressions.Password, ErrorMessage = SystemMessage.InvalidPassword)]
        public string ConfirmPassword { get; set; }
    }
}
