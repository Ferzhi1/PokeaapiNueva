using Microsoft.AspNetCore.Mvc;
using PokeApiNueva.Models;
using PokeApiNueva.Services;
namespace PokeApiNueva.Controllers
{
    public class PasswordRecoveryController : Controller
    {
        private readonly PasswordRecoveryService _service;

        public PasswordRecoveryController(PasswordRecoveryService service)
        {
            _service = service;
        }
        [HttpGet]
        public IActionResult RequestReset()
        {
            return View();

        }
        [HttpPost]
        public async Task<IActionResult> RequestReset(ForgotPasswordRecovery model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = await _service.FindUserByEmailAsync(model.Email, model.Answer);
            if (user == null)
            {
                ModelState.AddModelError(string.Empty, "Email or answer incorrect.");
                return View(model);
            }
            await _service.GenerateAndSetResetTokensAsync(user);
            return RedirectToAction("ResetPassword", new { token = user.ResetPasswordToken });

        }
        [HttpGet]
        public IActionResult RequestResetConfirmation()
        {
            return View();
        }
        [HttpGet]
        public IActionResult ResetPassword(string token)
        {
            if (string.IsNullOrEmpty(token))
            {
                return View("Error");
            }
            return View(new ResetPasswordViewModel { ResetToken = token });
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = await _service.FindUserByResetTokenAsync(model.ResetToken);
            if (user == null)
            {
                ModelState.AddModelError("", "The password reset link is invalid or has expired.");
                return View(model);
            }
            await _service.ResetPasswordAsync(user, model.NewPassword);
            return RedirectToAction("ResetPasswordConfirmation");
        }
        [HttpGet]
        public IActionResult ResetPasswordConfirmation()
        {
            return View();
        }

    }
}
