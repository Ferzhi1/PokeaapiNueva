using PokeApiNueva.Data;
using PokeApiNueva.Models;


namespace PokeApiNueva.Services
{
    public class AdminService
    {
        private readonly ApiDbContext _context;

        public AdminService(ApiDbContext context)
        {
            _context = context;
        }

        public async Task<bool> SwitchRoleAsync(int userId, string newRole)
        {
            var user = await _context.Userpkmns.FindAsync(userId);
            if (user == null) return false;

            if (Enum.TryParse<Roles>(newRole, out var parsedRole))
            {
                user.Role = parsedRole;
            }
            else
            {
                throw new ArgumentException("Invalid Role");
            }

            _context.Userpkmns.Update(user);
            await _context.SaveChangesAsync();
            return true;
        }

        public List<Userpkm> UsersList => _context.Userpkmns.ToList();

    }
}


