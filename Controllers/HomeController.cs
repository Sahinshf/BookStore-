using Microsoft.AspNetCore.Mvc;
namespace LoginRegisterProject.Controllers;

public class HomeController : Controller
{
    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Success()
    {
        return View();
    }
}
