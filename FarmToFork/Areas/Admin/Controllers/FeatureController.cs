using FarmToFork.Models;
using FarmToFork.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FarmToFork.Areas.Admin.Controllers;

[Area("Admin")]
public class FeatureController : Controller
{
    private readonly IRepository<Feature> _repository;

    public FeatureController(IRepository<Feature> repository)
    {
        _repository = repository;
    }

    public async Task<IActionResult> Index()
    {
        var models = await _repository.GetAll().ToListAsync();
        return View(models);
    }

    public IActionResult Add()
    {
        return View();
    }

}