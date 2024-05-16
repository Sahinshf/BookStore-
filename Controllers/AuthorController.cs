using Microsoft.AspNetCore.Mvc;

namespace LoginRegisterProject.Controllers;

public class AuthorController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}
