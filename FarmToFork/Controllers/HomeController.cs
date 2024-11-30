using Microsoft.AspNetCore.Mvc;

namespace FarmToFork.Controllers;

public class HomeController : Controller
{

    public IActionResult Index()
    {
        return View();
    }

    

    
}
