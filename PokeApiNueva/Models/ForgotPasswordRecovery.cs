using System.ComponentModel.DataAnnotations;

namespace PokeApiNueva.Models
{
    public class ForgotPasswordRecovery
    {
        [Required (ErrorMessage="Email is required.")]
        [EmailAddress(ErrorMessage="Please,Enter a valid email.")]
        public string Email { get; set; }
        [Required(ErrorMessage ="Security answer is required.")]
        public string Answer { get; set; }
    }
}
