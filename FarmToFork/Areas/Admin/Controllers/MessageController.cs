
using FarmToFork.Models;
using FarmToFork.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FarmToFork.Areas.Admin.Controllers;
[Area("Admin")]

public class MessageController : Controller
{
    private readonly IRepository<Message> _repository;

    public MessageController(IRepository<Message> repository)
    {
        _repository = repository;
    }

    [HttpGet]
    [Authorize(Roles = "Admin,SuperAdmin")]
    public async Task<IActionResult> Index()
    {
        List<Message>messages = await _repository.GetAll().Include(x=>x.AppUser).ToListAsync();
        return View(messages);
    }

    public async Task<IActionResult> Details(int id)
    {
        var result = await _repository.GetAll().Where(x => x.Id == id).Include(x=>x.AppUser).FirstOrDefaultAsync();
        if (result == null)
        {
            return Json(new { message = "Not found." });
        }
        return Json(result);
    }
    [Authorize(Roles = "Admin,SuperAdmin")]
    public async Task<IActionResult> Remove(int id)
    {
        await _repository.RemoveAsync(id);
        await _repository.SaveAsync();
        return RedirectToAction("Index");
    }
}