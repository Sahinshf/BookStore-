using Microsoft.AspNetCore.Mvc;

namespace LoginRegisterProject.Controllers;

public class CartController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}
