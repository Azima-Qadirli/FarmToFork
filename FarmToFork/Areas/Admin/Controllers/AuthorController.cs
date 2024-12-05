using FarmToFork.Models;
using FarmToFork.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FarmToFork.Areas.Admin.Controllers;
[Area("Admin")]
public class AuthorController : Controller
{
    private readonly IWebHostEnvironment _webHostEnvironment;
    private readonly IRepository<Author> _repository;

    public AuthorController(IWebHostEnvironment webHostEnvironment, IRepository<Author> repository)
    {
        _webHostEnvironment = webHostEnvironment;
        _repository = repository;
    }

    // GET
    public async Task<IActionResult> Index()
    {
        var author = await _repository.GetAll().ToListAsync();
        return View(author);
    }
    [HttpGet]
    public IActionResult Add()
    {
        return View();
    }
    [HttpPost]
    public async Task<IActionResult> Add(Author author)
    {
        string fileName = Guid.NewGuid().ToString() + author.File.FileName;

        string path = _webHostEnvironment.WebRootPath + "/images/authors/"+fileName;
        using (FileStream stream = System.IO.File.Open(path, FileMode.Create))
        {
            await author.File.CopyToAsync(stream);
        };
        author.FileName = fileName;
        await _repository.AddAsync(author);
        await _repository.SaveAsync();
        return RedirectToAction("Index");
    }
    [HttpGet]
    public async Task<IActionResult> Update(int id)
    {
        var data = await _repository.GetAsync(id);
        return View(data);
    }

    [HttpPost]
    public async Task<IActionResult> Update(int id, Author author)
    {
        var updatedAuthor = await _repository.GetAsync(id);
        updatedAuthor.Name = author.Name;
        updatedAuthor.Description = author.Description;
        if (author.File is not null)
        {
            string basePath = _webHostEnvironment.WebRootPath + "/images/authors";
            System.IO.File.Delete(basePath+updatedAuthor.FileName);
            string fileName = Guid.NewGuid() + author.File.FileName;

            string path = basePath +fileName;
            using (FileStream stream = System.IO.File.Open(path, FileMode.Create))
            {
                await author.File.CopyToAsync(stream);
            };
            updatedAuthor.FileName = fileName;
            
        }

        _repository.Update(updatedAuthor);
        await _repository.SaveAsync();
        return RedirectToAction("Index");
    }

    public async Task<IActionResult> Remove(int id)
    {
        var author = await _repository.GetAsync(id);
        _repository.RemoveAsync(id);
        System.IO.File.Delete( _webHostEnvironment.WebRootPath + "/images/authors/"+ author.FileName);
        await _repository.SaveAsync();
        return RedirectToAction("Index");
    }
}