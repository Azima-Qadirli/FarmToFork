using FarmToFork.Models;
using FarmToFork.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FarmToFork.Areas.Admin.Controllers;

[Area("Admin")]
public class BlogController : Controller
{
    private readonly IRepository<Blog> _repository;
    private readonly IWebHostEnvironment _webHostEnvironment;
    private readonly IRepository<Category> _categoryRepository;
    private readonly IRepository<Tag> _tagRepository;
    private readonly IRepository<BlogTag> _blogTagRepository;
    public BlogController(IRepository<Blog> repository, IWebHostEnvironment webHostEnvironment, IRepository<Category> categoryRepository, IRepository<Tag> tagRepository, IRepository<BlogTag> blogTagRepository)
    {
        _repository = repository;
        _webHostEnvironment = webHostEnvironment;
        _categoryRepository = categoryRepository;
        _tagRepository = tagRepository;
        _blogTagRepository = blogTagRepository;
    }

    public async Task<IActionResult> Index()
    {
        var models = await _repository.GetAll().Include(x=>x.Category).ToListAsync();
        return View(models);
    }
[HttpGet]
    public async  Task<IActionResult> Add()
    {
        ViewBag.Categories = await _categoryRepository.GetAll().ToListAsync();
        ViewBag.Tags = await _tagRepository.GetAll().ToListAsync();
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Add(Blog blog)
    {
        string fileName = Guid.NewGuid() + blog.File.FileName;
        string path = _webHostEnvironment.WebRootPath + "/images/blogs/"+fileName;
        using (FileStream fileStream = System.IO.File.Open(path,FileMode.Create))
        {
            await blog.File.CopyToAsync(fileStream);
        }
        blog.FileName = fileName;
        foreach (var tagId in blog.TagIDs)
        {
            await _blogTagRepository.AddAsync(new BlogTag
            {
                Blog = blog, 
                TagId = tagId
            });
        }
        
        await _repository.AddAsync(blog);
        await _repository.SaveAsync();
        return RedirectToAction("Index");
    }

    [HttpGet]
    public async Task<IActionResult> Update(int id)
    {
        
        var blog = await _repository.GetAsync(id);
        return View(blog);
    }

    [HttpPost]
    public async Task<IActionResult> Update(int id,Blog blog)
    {
        if (!ModelState.IsValid)
        {
            return View(); 
        }
        var updatedBlog = await _repository.GetAsync(id);
        //updatedBlog.Name = blog.Name;
        
        _repository.Update(updatedBlog);
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