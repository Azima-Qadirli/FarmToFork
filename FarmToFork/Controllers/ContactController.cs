using FarmToFork.Models;
using FarmToFork.Repositories.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace FarmToFork.Controllers;

public class ContactController : Controller
{
    private readonly IRepository<Message> _messageRepository;
    private readonly UserManager<AppUser> _userManager;
    public ContactController(IRepository<Message> messageRepository, UserManager<AppUser> userManager)
    {
        _messageRepository = messageRepository;
        _userManager = userManager;
    }


    // GET
    public IActionResult Index()
    {
        return View();
    }
[HttpPost]
    public async Task<IActionResult> SendMessage([FromBody] Message message)
    {
        if (!ModelState.IsValid)
        {
            return Json(new { successCode=400, message = "Please check your inputs." });
        }
        try
        {
            var user = await _userManager.FindByNameAsync(User.Identity?.Name);
            message.AppUserId = user.Id;
        }
        catch
        {
            return Json(new { successCode=404, message = "Firstly,you have to login." });
        }
        await _messageRepository.AddAsync(message);
        await _messageRepository.SaveAsync();
        return Json(new { successCode=201, message = "Your message has been sent." });
    }
}