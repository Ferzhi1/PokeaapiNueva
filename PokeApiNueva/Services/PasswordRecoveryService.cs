using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.EntityFrameworkCore;
using PokeApiNueva.Data;
using PokeApiNueva.Models;

namespace PokeApiNueva.Services
{
    public class PasswordRecoveryService
    {
        private readonly ApiDbContext _context;
        public PasswordRecoveryService(ApiDbContext context)
        {
            _context = context;
        }

        public async Task<Userpkm?> FindUserByEmailAsync(string email, string SecurityAnswer)
        {
            var user = await _context.Userpkmns.FirstOrDefaultAsync(u => u.Email == email);
            if (user != null && BCrypt.Net.BCrypt.Verify(SecurityAnswer, user.SecurityAnswer))
            {
                return user;

            }
            return null;

        }
        public async Task GenerateAndSetResetTokensAsync(Userpkm user) 
        {
            user.ResetPasswordToken=Guid.NewGuid().ToString();
            user.ResetPasswordTokenExpiry = DateTime.UtcNow.AddHours(1);
            _context.Userpkmns.Update(user);
            await _context.SaveChangesAsync();
        }
        public async Task<Userpkm?>FindUserByResetTokenAsync(string token) 
        {
            return await _context.Userpkmns.FirstOrDefaultAsync(
                u => u.ResetPasswordToken == token && u.ResetPasswordTokenExpiry > DateTime.UtcNow);


        }
        public async Task ResetPasswordAsync(Userpkm user, string newPassword) 
        {
            if (user==null)
            {
                Console.WriteLine("Error:User is null");
                return;
                
            }
            if (string.IsNullOrEmpty(newPassword))
            {
                Console.WriteLine("Error the new password is empty.");
                return;
                
            }
            _context.Attach(user);
            user.Password = BCrypt.Net.BCrypt.HashPassword(newPassword);
            user.ResetPasswordToken=null;
            user.ResetPasswordTokenExpiry = null;

            _context.Entry(user).Property(p => p.Password).IsModified = true;
            _context.Entry(user).Property(p=>p.ResetPasswordTokenExpiry).IsModified = true;

            foreach (var entry in _context.ChangeTracker.Entries())
            {
                Console.WriteLine($"Entity :{entry.Entity.GetType().Name},State:{entry.State}");
                
            }
            await _context.SaveChangesAsync();
            await _context.Entry(user).ReloadAsync();
        }
    }
}
