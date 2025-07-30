using PokeApiNueva.Data;
using Microsoft.EntityFrameworkCore;
using PokeApiNueva.Models;
using System.Runtime.InteropServices;

namespace PokeApiNueva.Services
{


    public class AuthService
    {
        private readonly ApiDbContext _context;
        public AuthService(ApiDbContext context)
        {
            _context = context;
        }
        public async Task<Userpkm> AuthenticateAsync(string email, string password)
        {
            var user = await _context.Userpkmns.FirstOrDefaultAsync(u => u.Email == email);
            if (user == null || !BCrypt.Net.BCrypt.Verify(password, user.Password))
                return null;

      
            if (!user.LastCreditDate.HasValue || user.LastCreditDate.Value.AddDays(1) <= DateTime.UtcNow)
            {
                user.Credits += 1;
                user.LastCreditDate = DateTime.UtcNow;
                _context.Userpkmns.Update(user);
                await _context.SaveChangesAsync();
            }

            return user;
        }

        public async Task RegisterAsync(string email,string name,string password,string question,string answer,string city) 
        {
            var hashedPassword=BCrypt.Net.BCrypt.HashPassword(password);
            var hashAnswer = BCrypt.Net.BCrypt.HashPassword(answer);
            var newUser = new Userpkm
            {
                Email = email,
                Name = name,
                Password = hashedPassword,
                SecurityQuestion = question,
                SecurityAnswer = hashAnswer,
                City= city
            };
            _context.Userpkmns.Add(newUser);
            await _context.SaveChangesAsync();

        }
    }
}
