using Microsoft.AspNetCore.Mvc;

namespace FarmToFork.Controllers;

public class AboutController : Controller
{
    // GET
    public IActionResult Index()
    {
        return View();
    }
}