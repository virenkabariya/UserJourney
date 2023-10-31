using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using UserJourney.Core.Constants;

namespace UserJourney.Repositories.EF
{
    [Table("Users")]
    public partial class Users
    {
        public Users()
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

        [StringLength(50)]
        public string PasswordResetToken { get; set; }
        public DateTime? LastTokenCreatedDate { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
    }
}
