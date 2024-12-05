using FarmToFork.Models;
using FarmToFork.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FarmToFork.Controllers;

public class BlogController : Controller
{
   private readonly IRepository<Blog> _repository;
   private readonly IRepository<Category> _categoryRepository;
   private readonly IRepository<Tag> _tagRepository;

   public BlogController(IRepository<Blog> repository, IRepository<Category> categoryRepository, IRepository<Tag> tagRepository)
   {
       _repository = repository;
       _categoryRepository = categoryRepository;
       _tagRepository = tagRepository;
   }

   public async  Task<IActionResult> Index()
    {
        var blogs = await _repository.GetAll().Include(x=>x.Author).ToListAsync();
        return View(blogs);
    }

    public async Task<IActionResult> Detail(int id)
    {
        ViewBag.Categories = await _categoryRepository.GetAll().Include(x=>x.Blogs).ToListAsync();
        ViewBag.Tags = await _tagRepository.GetAll().ToListAsync();
        ViewBag.Blogs= await _repository.GetAll().Include(x=>x.Author).OrderByDescending(x=>x.CreatedAt).Take(3).ToListAsync();
        var blog = await _repository.GetAll().Where(x => x.Id == id).Include(x=>x.BlogTags).ThenInclude(x=>x.Tag).Include(x=>x.Author).FirstOrDefaultAsync();
        return View(blog);
    }
}