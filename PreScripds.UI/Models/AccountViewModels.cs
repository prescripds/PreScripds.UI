using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PreScripds.UI.Models
{
    public class ExternalLoginConfirmationViewModel
    {
        [Required]
        [Display(Name = "User name")]
        public string UserName { get; set; }
    }

    public class ManageUserViewModel
    {
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Current password")]
        public string OldPassword { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "New password")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm new password")]
        [Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }

    public class LoginViewModel
    {
        [Required(ErrorMessage = "User name is required.")]
        [Display(Name = "User name")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }
        [Display(Name = "Captcha")]
        public string Captcha { get; set; }
        public bool? IsCaptchaDisplay { get; set; }
        public string CaptchaUserInput { get; set; }
        public string CaptchaValid { get; set; }
    }

    public class ForgotPasswordViewModel
    {
        public string UserName { get; set; }
        public List<RecoveryModeViewModel> RecoveryModeViewModels { get; set; }
        public int RecoveryMode { get; set; }
        public string UserInput { get; set; }
        public string SelectedName { get; set; }
        public string SecurityQuestion { get; set; }
        public string SecurityAnswer { get; set; }
    }

    public class RecoveryModeViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }

    }

    public enum RecoveryMode
    {
        SendViaEmail,
        SendViaSMS,
        AnswerSecurityQuestion,
        SendViaAlternateEmail,
        SendViaAlternateMobileSMS
    }
}
