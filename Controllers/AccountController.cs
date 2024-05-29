using BookStore.Models;
using BookStore.Utils;
using BookStore.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
namespace BookStore.Controllers;

public class AccountController : Controller
{
    private readonly UserManager<AppUser> _userManager; // CRUD əməliyyatları üçün istifadə olunur
    private readonly SignInManager<AppUser> _signInManager; // Login logout əməliiytları üçün istifadə olunur
    private readonly RoleManager<IdentityRole> _roleManager; // Role əməliyyatları üçün istifadə olunur

    public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, RoleManager<IdentityRole> roleManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _roleManager = roleManager;
    }

    public IActionResult Register()
    {
        if (User.Identity.IsAuthenticated)
        {
            return BadRequest(); // Login olub olmadığını yoxlayır
        }

        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Register(RegisterViewModel registerViewModel)
    {
        if (User.Identity.IsAuthenticated)
        {
            return BadRequest(); // Login olub olmadığını yoxlayır
        }

        if (!ModelState.IsValid)
        {
            return View();
        }

        AppUser newUser = new()
        {
            UserName = registerViewModel.Username,
            Email = registerViewModel.Email,
        };

        var identityResult = await _userManager.CreateAsync(newUser, registerViewModel.Password); // user create olunur Passwordu method hash`ləyir

        if (!identityResult.Succeeded) // identityResult.Succeeded  user create olunub ya da olunmayıb onu retur edir
        {
            foreach (var error in identityResult.Errors)
            {
                ModelState.AddModelError("", error.Description);

            }
            return View();
        }

        await _userManager.AddToRoleAsync(newUser, "Member"); // Register olan user ilk halda Member olur

        return RedirectToAction(nameof(Login));
    }

    public IActionResult Login()
    {
        if (User.Identity.IsAuthenticated)
        {
            return BadRequest(); // Login olub olmadığını yoxlayır
        }

        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginViewModel loginViewModel)
    {
        if (User.Identity.IsAuthenticated) // Login olub olmadığını yoxlayır
            return BadRequest();


        if (!ModelState.IsValid) return View();

        var user = await _userManager.FindByNameAsync(loginViewModel.UserName); //Find user by user name
        if (user == null)
        {
            ModelState.AddModelError("", "Password or Username invalid ");
            return View();
        }

        if (!user.IsActive)
        {
            ModelState.AddModelError("", "User is blocked ");
            return View();
        }

        //var signInResult = await _signInManager.PasswordSignInAsync(loginViewModel.UserName, loginViewModel.Password, loginViewModel.RememberMe, true)
        var signInResult = await _signInManager.PasswordSignInAsync(loginViewModel.UserName, loginViewModel.Password, true, true); //useri login etmək

        if (signInResult.IsLockedOut)
        {
            ModelState.AddModelError("", "Your account is blocked temporary");
        }
        if (!signInResult.Succeeded)
        {
            ModelState.AddModelError("", "Password or Username invalid ");
            return View();
        }


        return RedirectToAction("Success", "Home");
    }

    public async Task<IActionResult> Logout()
    {
        if (!User.Identity.IsAuthenticated)
        {
            return BadRequest();
        }

        await _signInManager.SignOutAsync(); // sign out
        return RedirectToAction("Index","Home");
    }

    public async Task<IActionResult> CreateRoles() // Bir dəfə istifadə olunur Role`ları yaratmaq üçün
    {
        foreach (var role in Enum.GetValues(typeof(RoleType))) // Eyni role`u təkrar yaratmamaq üçündü bu kod block`u lazımdır
        {
            if (!await _roleManager.RoleExistsAsync(role.ToString()))
            {

                await _roleManager.CreateAsync(new IdentityRole { Name = role.ToString() });
            }
        }
        return Json("Ok");
    }
}
