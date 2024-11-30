using Microsoft.AspNetCore.Mvc;

namespace FarmToFork.Controllers;

public class ProductController : Controller
{
    // GET
    public IActionResult Index()
    {
        return View();
    }
}