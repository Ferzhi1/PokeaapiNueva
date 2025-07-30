using Microsoft.AspNetCore.Mvc.Rendering;

namespace PokeApiNueva.Models
{
    public class RegisterRequest
    {
        public string Email { get; set; }

        public string Name { get; set; }

        public string Password { get; set; }

        public string SecurityAnswer { get; set; }

        public string SecurityQuestion { get; set; }

        public string City { get; set; }
     
    }

}
