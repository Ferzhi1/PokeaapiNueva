using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PokeApiNueva.Data;

namespace PokeApiNueva.Services
{
    

    public class TradeService
    {
        private readonly ApiDbContext _context;
        public TradeService(ApiDbContext context)
        {
            _context = context;
        }

        public async Task<bool> TradePokemonAsync(int fromUserId, int toUserId, int pokemonId)
        {
            Console.WriteLine($"Iniciando trade: {fromUserId} => {toUserId}, PokémonID: {pokemonId}");

            var user = await _context.Userpkmns.FirstOrDefaultAsync(u => u.Id == fromUserId);
            if (user == null)
            {
                Console.WriteLine("Usuario no encontrado.");
                return false;
            }
            if (user.Credits <= 0)
            {
                Console.WriteLine("Usuario sin créditos.");
                return false;
            }

            var pokemon = await _context.CollectionUserPkms.FirstOrDefaultAsync(p => p.UserId == fromUserId && p.Id == pokemonId);
            if (pokemon == null)
            {
                Console.WriteLine("Pokémon no encontrado o no pertenece al usuario.");
                return false;
            }

            pokemon.UserId = toUserId;
            user.Credits--;

            await _context.SaveChangesAsync();
            Console.WriteLine($"Trade completado: Pokémon ahora pertenece a usuario {toUserId}");
            return true;
        }
      


    }
}
