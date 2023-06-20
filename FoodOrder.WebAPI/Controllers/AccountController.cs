using FoodOrder.DTO;
using FoodOrder.Persistance;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace FoodOrder.WebAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly SignInManager<Employees> _signInManager;

        public AccountController(SignInManager<Employees> signInManager)
        {
            _signInManager = signInManager;
        }

        [HttpPost]
        public async Task<IActionResult> Login([FromBody] LoginDto user)
        {
            if (_signInManager.IsSignedIn(User))
                await _signInManager.SignOutAsync();

            var result = await _signInManager.PasswordSignInAsync(user.UserName, user.Password,
                isPersistent: false, lockoutOnFailure: false);

            if (result.Succeeded)
            {
                return Ok();
            }

            return Unauthorized("A bejelentkezés sikertelen!");
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();

            return Ok();
        }
    }
}
