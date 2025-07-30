namespace PokeApiNueva.Models
{
    public class Userpkm
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string? ResetPasswordToken { get; set; }
        public DateTime? ResetPasswordTokenExpiry { get; set; }

        public string SecurityQuestion { get; set; }

        public string SecurityAnswer { get; set; }

        public int Credits { get; set; } = 5;

        public DateTime?  LastCreditDate {get; set;}

        public string City { get; set; }
    }
}
