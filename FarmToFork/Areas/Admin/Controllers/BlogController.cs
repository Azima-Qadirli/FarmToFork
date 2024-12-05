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
    private readonly IRepository<Author> _authorRepository;
    public BlogController(IRepository<Blog> repository, IWebHostEnvironment webHostEnvironment, IRepository<Category> categoryRepository, IRepository<Tag> tagRepository, IRepository<BlogTag> blogTagRepository, IRepository<Author> authorRepository)
    {
        _repository = repository;
        _webHostEnvironment = webHostEnvironment;
        _categoryRepository = categoryRepository;
        _tagRepository = tagRepository;
        _blogTagRepository = blogTagRepository;
        _authorRepository = authorRepository;
    }

    public async Task<IActionResult> Index()
    {
        var models = await _repository.GetAll().Include(x=>x.Category).Include(x=>x.Author).ToListAsync();
        return View(models);
    }
[HttpGet]
    public async  Task<IActionResult> Add()
    {
        ViewBag.Categories = await _categoryRepository.GetAll().ToListAsync();
        ViewBag.Tags = await _tagRepository.GetAll().ToListAsync();
        ViewBag.Authors = await _authorRepository.GetAll().ToListAsync();
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Add(Blog blog)
    {
        string fileName = Guid.NewGuid() + blog.File.FileName;
        string path = _webHostEnvironment.WebRootPath + "/images/blogs/" +fileName;
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
        ViewBag.Categories = await _categoryRepository.GetAll().ToListAsync();
        ViewBag.Tags = await _tagRepository.GetAll().Include(x=>x.BlogTags).ToListAsync();
        ViewBag.Authors = await _authorRepository.GetAll().ToListAsync();
        
        var blog = await _repository.GetAsync(id);
        return View(blog);
    }

    [HttpPost]
    public async Task<IActionResult> Update(int id,Blog blog)
    {
        var updatedBlog = await _repository.GetAll().Include(x=>x.BlogTags).FirstOrDefaultAsync(x => x.Id == id);
        updatedBlog.Title1 = blog.Title1;
        updatedBlog.Description1 = blog.Description1;
        updatedBlog.Title2 = blog.Title2;
        updatedBlog.Description2 = blog.Description2;
        updatedBlog.CategoryId = blog.CategoryId;
        updatedBlog.AuthorId = blog.AuthorId;
        updatedBlog.Paragraph = blog.Paragraph;

        if (blog.File != null)
        {
            string basePath = _webHostEnvironment.WebRootPath + "/images/blogs/";
            System.IO.File.Delete(basePath + updatedBlog.FileName);
            
            string fileName = Guid.NewGuid() +  blog.File.FileName;
            string path = basePath + fileName;
    
            using (FileStream stream = System.IO.File.Open(path, FileMode.Create))
            {
                await blog.File.CopyToAsync(stream);
            };
            updatedBlog.FileName = fileName;
        }
        List<BlogTag> removedTags = new List<BlogTag>();

        foreach (var blogTag in updatedBlog.BlogTags) //2 ,3 
        {
            bool result = false;
            foreach (var tagId in blog.TagIDs) //1,2
                if (blogTag.TagId == tagId)
                    result = true;
            
            if (!result)
                updatedBlog.BlogTags.Remove(blogTag);
        }

        foreach (var blogTagID in blog.TagIDs)
        {
            if (!updatedBlog.BlogTags.Any(x => x.TagId == blogTagID)) 
            {
                updatedBlog.BlogTags.Add(new BlogTag()
                {
                    TagId = blogTagID,
                    BlogId = id
                });
            }
        }
        _repository.Update(updatedBlog);
        await _repository.SaveAsync();
        return RedirectToAction("Index");
    }

    public async Task<IActionResult> Remove(int id)
    {
        var blog = await _repository.GetAsync(id);
        _repository.RemoveAsync(id);
        
        System.IO.File.Delete(_webHostEnvironment.WebRootPath + "/images/blogs/" + blog.FileName);
        await _repository.SaveAsync();
        return RedirectToAction("index");
    }
}