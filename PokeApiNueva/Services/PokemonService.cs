using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using PokeApiNueva.Data;
using PokeApiNueva.Models;

namespace PokeApiNueva.Services
{
    public class PokemonService
    {
        private readonly HttpClient _client;
        private readonly ApiDbContext _context;
        public PokemonService(HttpClient client, ApiDbContext context)
        {
            _context = context;
            _client = client;
        }

        public async Task<Pokemon?> GetPokemonAsync(string pokemonName)
        {
            try
            {
                var response = await _client.GetAsync($"pokemon/{pokemonName.ToLower()}");
                response.EnsureSuccessStatusCode();

                var pokemon = await response.Content.ReadFromJsonAsync<Pokemon>();
                return pokemon;

            }
            catch (HttpRequestException e)
            {
                Console.WriteLine($"Request Error:{e.Message}");
                if (e.StatusCode.HasValue)
                {
                    Console.WriteLine($"StatusCode :{e.StatusCode}");


                }
                return null;

                throw;

            }
            catch (Exception e)
            {
                Console.WriteLine($"An unexpected error ocurred:{e.Message}");
                return null;
            }


        }
        public async Task<int> GetTotalPokemonsAsync()
        {
            var response = await _client.GetAsync("pokemon?limit=1");
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadFromJsonAsync<JsonElement>();
            int count = json.GetProperty("count").GetInt32();
            return count;
        }

        public async Task<Pokemon?> GetRandomPokemonAsync()
        {
            try
            {
                int total = await GetTotalPokemonsAsync();
                var random = new Random();
                int randomId = random.Next(1, total + 1);

                var response = await _client.GetAsync($"pokemon/{randomId}");
                response.EnsureSuccessStatusCode();

                var pokemon = await response.Content.ReadFromJsonAsync<Pokemon>();
                return pokemon;
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error getting random Pokémon: {e.Message}");
                return null;
            }
        }
        public async Task<string?> TryCaptureAsync(int userId)
        {
            var user = await _context.Userpkmns.FindAsync(userId);
            if (user == null || user.Credits < 1)
                return null;


            user.Credits -= 1;
            _context.Userpkmns.Update(user);


            var pokemon = await GetRandomPokemonAsync();
            if (pokemon == null) return null;


            var existing = await _context.CollectionUserPkms
                .FirstOrDefaultAsync(p => p.UserId == userId && p.Name == pokemon.Name);

            if (existing != null)
            {
                existing.Count += 1;
                _context.CollectionUserPkms.Update(existing);
            }
            else
            {
                _context.CollectionUserPkms.Add(new CollectionUserPkm
                {
                     PokemonId=pokemon.Id,
                    UserId = userId,
                    Name = pokemon.Name,
                    Count = 1,
                    CaughtAt = DateTime.UtcNow
                });
            }

            await _context.SaveChangesAsync();
            return pokemon.Name;
        }

    }
}
