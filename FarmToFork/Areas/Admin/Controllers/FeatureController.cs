using FarmToFork.Models;
using FarmToFork.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FarmToFork.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = "Admin,SuperAdmin")]
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
[HttpGet]
    public IActionResult Add()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Add(Feature feature)
    {
        if (!ModelState.IsValid)
        {
            return View();
        }
        await _repository.AddAsync(feature);
        await _repository.SaveAsync();
        return RedirectToAction("Index");
    }

    [HttpGet]
    public async Task<IActionResult> Update(int id)
    {
        
        var feature = await _repository.GetAsync(id);
        return View(feature);
    }

    [HttpPost]
    public async Task<IActionResult> Update(int id,Feature feature)
    {
        if (!ModelState.IsValid)
        {
            return View(); 
        }
        var updatedFeature = await _repository.GetAsync(id);
        updatedFeature.Title = feature.Title;
        updatedFeature.Description = feature.Description;
        updatedFeature.Icon = feature.Icon;
        
        _repository.Update(updatedFeature);
        await _repository.SaveAsync();
        return RedirectToAction("Index");
    }

    public async Task<IActionResult> Remove(int id)
    {
        await _repository.RemoveAsync(id);
        await _repository.SaveAsync();
        return RedirectToAction("Index");
    }
}