using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PokeApiNueva.Data;
using PokeApiNueva.Models;
using PokeApiNueva.Services;

[Authorize]
public class PokemonController : Controller
{
    private readonly PokemonService _service;
    private readonly ApiDbContext _context;

    public PokemonController(PokemonService service,ApiDbContext context)
    {
        _context = context;
        _service = service;
    }
    public async Task<IActionResult> Index(string pokemonName = "pikachu")
    {
        if (string.IsNullOrWhiteSpace(pokemonName))
        {
            pokemonName = "pikachu";
        }
        var pokemon = await _service.GetPokemonAsync(pokemonName);
        if (pokemon == null)
        {
            return NotFound();

        }
        return View(pokemon);
    }
    public async Task<IActionResult> Details(string pokemonName)
    {
        var user = await GetCurrentUserAsync();
        if (user == null) return Unauthorized();

        var hasPokemon = await _context.CollectionUserPkms
            .AnyAsync(p => p.UserId == user.Id && p.Name.ToLower() == pokemonName.ToLower() && p.Count > 0);

        if (!hasPokemon)
        {
            TempData["Error"] = $"You haven't caught a Pokémon named '{pokemonName}'.";
            return RedirectToAction("Album");
        }

        var pokemon = await _service.GetPokemonAsync(pokemonName);
        if (pokemon == null)
        {
            TempData["Error"] = $"No Pokémon found with the name '{pokemonName}'.";
            return RedirectToAction("Album");
        }

        return View(pokemon);
    }


    public async Task<IActionResult> Random()
    {
        var pokemon = await _service.GetRandomPokemonAsync();
        if (pokemon == null)
        {
            return NotFound();
        }

        return View("Index", pokemon); 
    }
    private async Task<Userpkm?> GetCurrentUserAsync()
    {
        var email = HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
        if (string.IsNullOrEmpty(email)) return null;
        return await _context.Userpkmns.FirstOrDefaultAsync(u => u.Email == email);
    }

    [HttpPost]
    public async Task<IActionResult> Capture()
    {
        var user = await GetCurrentUserAsync();
        if (user == null) return Unauthorized();

        var result = await _service.TryCaptureAsync(user.Id);
        if (result == null)
        {
            TempData["Error"] = "You don’t have enough credits to catch a Pokémon.";
            return RedirectToAction("Index", "Profile"); 
        }

        TempData["Success"] = $"You caught {result.ToUpper()}!";
        return RedirectToAction("Details", new { pokemonName = result });
    }


    public async Task<IActionResult> Album(int page = 1)
    {
        var user = await GetCurrentUserAsync();
        if (user == null) return Unauthorized();

        var capturedList = await _context.CollectionUserPkms
            .Where(p => p.UserId == user.Id)
            .ToListAsync();

        var allIds = Enumerable.Range(1, 1000);
        var fullList = allIds.Select(id =>
        {
            var match = capturedList.FirstOrDefault(p => p.PokemonId == id);
            var hasPokemon = match != null && match.Count > 0;

            return new CollectionUserPkm
            {
                PokemonId = id,
                Name = hasPokemon ? match.Name : "???",
                Count = hasPokemon ? match.Count : 0,
                CaughtAt = hasPokemon ? match.CaughtAt : DateTime.MinValue
            };
        }).ToList();

        int pageSize = 50;
        int totalItems = fullList.Count;
        int totalPages = (int)Math.Ceiling((double)totalItems / pageSize);

        var pagedList = fullList
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToList();

        ViewData["CurrentPage"] = page;
        ViewData["TotalPages"] = totalPages;

        return View(pagedList);
    }

}






