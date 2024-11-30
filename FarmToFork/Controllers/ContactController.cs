using Microsoft.AspNetCore.Mvc;

namespace FarmToFork.Controllers;

public class ContactController : Controller
{
    // GET
    public IActionResult Index()
    {
        return View();
    }
}