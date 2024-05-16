using Microsoft.AspNetCore.Mvc;

namespace LoginRegisterProject.Controllers;

public class ContactController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}
