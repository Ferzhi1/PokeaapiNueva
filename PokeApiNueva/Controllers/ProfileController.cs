using Microsoft.AspNetCore.Mvc;
using PokeApiNueva.Data;
using PokeApiNueva.Models;
using PokeApiNueva.Services;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;

public class ProfileController : Controller
{
    private readonly ApiDbContext _context;
    private readonly WeatherService _weatherService;

    public ProfileController(ApiDbContext context, WeatherService weatherService)
    {
        _context = context;
        _weatherService = weatherService;
    }

    private async Task<Userpkm?> GetCurrentUserAsync()
    {
        var email = HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
        if (string.IsNullOrEmpty(email)) return null;
        return await _context.Userpkmns.FirstOrDefaultAsync(u => u.Email == email);
    }

    public async Task<IActionResult> Index()
    {
        var user = await GetCurrentUserAsync();
        if (user == null) return Unauthorized();

        string weather = "Weather No available";
        if (!string.IsNullOrEmpty(user.City))
        {
            weather = await _weatherService.GetWeatherAsync(user.City);
        }

        ViewBag.User = user;
        ViewBag.Weather = weather;

        return View();
    }
}
