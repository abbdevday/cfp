using System;
using System.ComponentModel.DataAnnotations;

namespace DevDayCFP.ViewModels
{
    public class ResetPasswordViewModel
    {
        public Guid UserId { get; set; }
        public Guid TokenId { get; set; }

        [Required]
        [StringLength(128, MinimumLength = 8, ErrorMessage = "Password lengths must be between 8 and 128 characters in length")]
        [RegularExpression(@"^(?=.*\d)(?=.*[a-z])(?=.*[A-Z]).{8,128}$", ErrorMessage = "Password must contain at least: one lowercase and one uppercase letter, one digit and be minimum 8 characters long")]
        public string Password { get; set; }

        [Required]
        public string ConfirmPassword { get; set; }
    }
}