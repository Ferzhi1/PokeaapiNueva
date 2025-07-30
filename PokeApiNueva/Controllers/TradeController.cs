using Microsoft.AspNetCore.Mvc;
using PokeApiNueva.Services;
using PokeApiNueva.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PokeApiNueva.Data;
using System.Security.Claims;

namespace PokeApiNueva.Controllers
{
    public class TradeController : Controller
    {
        private readonly ApiDbContext _context;
        private readonly TradeService _tradeService;
        public TradeController(TradeService tradeService,ApiDbContext context)
        {
            _tradeService = tradeService;
            _context = context;
        }



        [HttpGet]
        public IActionResult Trade()
        {
            var email = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
            if (string.IsNullOrEmpty(email))
            {
                return Unauthorized(); // The user is not authenticated
            }

            var currentUser = _context.Userpkmns.FirstOrDefault(u => u.Email == email);
            if (currentUser == null)
            {
                return NotFound("User not found.");
            }

            ViewBag.Users = new SelectList(
                _context.Userpkmns.Where(u => u.Id != currentUser.Id), "Id", "Name");

            var userPokemons = _context.CollectionUserPkms
                .Where(p => p.UserId == currentUser.Id)
                .ToList();

            ViewBag.Pokemons = new SelectList(userPokemons, "Id", "Name");

            var model = new TradeRequest { fromUserId = currentUser.Id };
            return View(model);
        }







        [HttpPost]
        public async Task<IActionResult> Trade(TradeRequest model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Error = "❌ Verifica la información ingresada.";
                return View(model);
            }

            bool result = await _tradeService.TradePokemonAsync(
                model.fromUserId,
                model.ToUserId,
                model.PokemonId);

            if (result)
            {
                ViewBag.Message = "✅ ¡Intercambio exitoso!";
            }
            else
            {
                ViewBag.Error = "❌ Falló el intercambio (verifica el Pokémon o los créditos disponibles).";
            }

            
            ViewBag.Users = new SelectList(_context.Userpkmns, "Id", "Name");
            ViewBag.Pokemons = new SelectList(_context.CollectionUserPkms, "Id", "Name");

            return View(model);
        }

    }
}
