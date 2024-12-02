using FarmToFork.Models;
using FarmToFork.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FarmToFork.Controllers;

public class HomeController : Controller
{
    private readonly IRepository<Feature> _repository;

    public HomeController(IRepository<Feature> repository)
    {
        _repository = repository;
    }

    public  async Task<IActionResult> Index()
    {
        var response = await _repository.GetAll().ToListAsync();
        return View(response);
    }

    

    
}
