using Microsoft.AspNetCore.Mvc;

namespace LoginRegisterProject.Controllers;

public class AboutController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}
