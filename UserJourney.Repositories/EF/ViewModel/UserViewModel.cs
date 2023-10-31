using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using UserJourney.Core.Constants;

namespace UserJourney.Repositories.EF.ViewModel
{
    public class UserViewModel
    {
        public UserViewModel()
        {
        }

        [Key]
        [Column("UserId")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserId { get; set; }

        [Required]
        [StringLength(50)]
        [RegularExpression(RegularExpressions.HtmlTag, ErrorMessage = SystemMessage.InvalidInput)]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required]
        [StringLength(50)]
        [RegularExpression(RegularExpressions.HtmlTag, ErrorMessage = SystemMessage.InvalidInput)]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [StringLength(100)]
        [Display(Name = "Email")]
        [Required]
        [RegularExpression(RegularExpressions.Email, ErrorMessage = SystemMessage.EmailErrorMessage)]
        public string Email { get; set; }

        [StringLength(50)]
        [Display(Name = "Phone Number")]
        [RegularExpression(RegularExpressions.PhoneNumber, ErrorMessage = SystemMessage.PhoneNumberErrorMessage)]
        public string PhoneNumber { get; set; }

        [Required]
        [StringLength(50)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        [RegularExpression(RegularExpressions.Password, ErrorMessage = SystemMessage.InvalidPassword)]
        public string Password { get; set; }

        [NotMapped]
        [StringLength(50)]
        [DataType(DataType.Password)]
        [Compare("Password")]
        [Display(Name = "Confirm Password")]
        [RegularExpression(RegularExpressions.Password, ErrorMessage = SystemMessage.InvalidPassword)]
        public string ConfirmPassword { get; set; }
    }
}
