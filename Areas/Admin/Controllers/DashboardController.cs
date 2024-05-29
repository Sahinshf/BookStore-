using Microsoft.AspNetCore.Mvc;

namespace BookStore.Areas.Admin.Controllers;

[Area("Admin")]
public class DashboardController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}
