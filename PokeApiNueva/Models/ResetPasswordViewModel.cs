using System.ComponentModel.DataAnnotations;

namespace PokeApiNueva.Models
{
    public class ResetPasswordViewModel
    {


        [Required(ErrorMessage = "New Passsword is required")]
        [DataType(DataType.Password)]
        [Display(Name = "New Password")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm New Password")]
        [Compare("NewPassword", ErrorMessage = "The new password and the confirmation do not match.")]
        public string? ConfirmNewPassword { get; set; }

        public string? ResetToken { get; set; }

    }
}
