using FarmToFork.Context;
using FarmToFork.Models;
using FarmToFork.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FarmToFork.Controllers;

public class ProductController : Controller
{
    private readonly FarmToForkDbContext _context;
    private readonly IBasketService _basketService;
    private readonly IHttpContextAccessor _httpContextAccessor;
    public ProductController(FarmToForkDbContext context, IBasketService basketService, IHttpContextAccessor httpContextAccessor)
    {
        _context = context;
        _basketService = basketService;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<IActionResult> Detail(int id)
    {
        Product? product = await _context.Products.Where(x => x.Id == id && !x.IsDeleted).FirstOrDefaultAsync();
        if (product == null)
        {
            return NotFound();
        }

        return View(product);
    }

    public async Task<IActionResult> AddBasket(int id,int? count)
    {
        await _basketService.AddBasket(id, count);
        return Json(new { status = 200 });
    }

    public async Task<IActionResult> GetAll()
    {
        var result = await _basketService.GetAll();
        return Json(result);
    }

    public async Task<IActionResult> Remove(int id)
    {
        await _basketService.Remove(id);
        return RedirectToAction("index","home");
    }
}