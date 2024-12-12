using FarmToFork.Context;
using FarmToFork.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace FarmToFork.Services;

public class BasketService:IBasketService
{
    private readonly FarmToForkDbContext _context;
    private readonly UserManager<AppUser> _userManager;
    private readonly IHttpContextAccessor _httpContextAccessor;
    public BasketService(FarmToForkDbContext context, UserManager<AppUser> userManager, IHttpContextAccessor httpContextAccessor)
    {
        _context = context;
        _userManager = userManager;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task AddBasket(int id, int? count)
    {
        if (!await _context.Products.AnyAsync(x => x.Id == id && !x.IsDeleted))
        {
            throw new Exception("Not found");
        }

        if (_httpContextAccessor.HttpContext.User.Identity.IsAuthenticated)
        {
            AppUser appUser = await _userManager.FindByNameAsync(_httpContextAccessor.HttpContext.User.Identity.Name);
            Basket? basket = await _context.Baskets.Include(x => x.BasketItems.Where(y => !y.IsDeleted))
                .Where(x => !x.IsDeleted && x.AppUserId == appUser.Id).FirstOrDefaultAsync();
            if (basket == null)
            {
                basket = new Basket()
                {
                    AppUserId = appUser.Id,
                    CreatedAt = DateTime.Now
                };
                await _context.AddAsync(basket);
                BasketItem basketItem = new BasketItem()
                {
                    Basket = basket,
                    Count = count??1,
                    CreatedAt = DateTime.Now,
                    ProductId = id
                };
                await _context.AddAsync(basketItem);
            }
            else
            {
                BasketItem basketItem = await _context.BasketItems.Where(x=>x.ProductId==id && !x.IsDeleted).FirstOrDefaultAsync();
                if (basketItem == null)
                {
                    basketItem = new BasketItem()
                    {
                        Basket = basket,
                        Count = count ?? 1,
                        CreatedAt = DateTime.Now,
                        ProductId = id
                    };
                    await _context.AddAsync(basketItem);
                }
                else
                {
                    basketItem.Count += count ?? 1;
                }
            }
            await _context.SaveChangesAsync();
        }
        else
        {
            var cookiesJson = _httpContextAccessor.HttpContext?.Request.Cookies["basket"];
            if (cookiesJson == null)
            {
                List<BasketItem> basketItems = new List<BasketItem>();
                BasketItem basketItem = new BasketItem()
                {
                    ProductId = id,
                    Count = count ?? 1
                };
                basketItems.Add(basketItem);
                cookiesJson = JsonConvert.SerializeObject(basketItems);
                _httpContextAccessor.HttpContext?.Response.Cookies.Append("basket", cookiesJson);
            }
            else
            {
                List<BasketItem>? basketItems = JsonConvert.DeserializeObject<List<BasketItem>>(cookiesJson);
                BasketItem? basketItem = basketItems.FirstOrDefault(x => x.ProductId == id);
                if (basketItem == null)
                {
                    BasketItem basketItem1 = new BasketItem()
                    {
                        ProductId = id,
                        Count = count ?? 1
                    };
                    basketItems.Add(basketItem1);
                }
                else
                {
                    basketItem.Count += count ?? 1;
                }
                cookiesJson = JsonConvert.SerializeObject(basketItems);
                _httpContextAccessor.HttpContext?.Response.Cookies.Append("basket", cookiesJson);
            }
        }
    }

    public async  Task<List<BasketItem>> GetAll()
    {
        if (_httpContextAccessor.HttpContext.User.Identity.IsAuthenticated)
        {
            AppUser appUser = await _userManager.FindByNameAsync(_httpContextAccessor.HttpContext.User.Identity.Name);
            Basket? basket = await _context.Baskets.
                Include(x => x.BasketItems.Where(y => !y.IsDeleted)).
                FirstOrDefaultAsync(x => x.AppUserId == appUser.Id && !x.IsDeleted);
            if (basket is not null)
            {
                List<BasketItem> basketItems = new List<BasketItem>();
                foreach (var item in basket.BasketItems)
                {
                    basketItems.Add(new BasketItem()
                    {
                        ProductId = item.ProductId,
                        Count = item.Count
                    });
                }
                return basketItems;
            }
        }
        else
        {
            var cookiesJson = _httpContextAccessor.HttpContext?.Request.Cookies["basket"];
            if (cookiesJson != null)
            {
                List<Basket>? baskets = JsonConvert.DeserializeObject<List<Basket>>(cookiesJson);
                List<BasketItem> basketItems = new List<BasketItem>();
                foreach (var item in basketItems)
                {
                    Product? product = await _context.Products.Where(x=>x.Id==item.ProductId && !x.IsDeleted).FirstOrDefaultAsync();
                    if (product is not null)
                    {
                        BasketItem basketItem = new BasketItem()
                        {
                            ProductId = item.ProductId,
                            Count = item.Count
                        };
                        basketItems.Add(basketItem);
                    }
                }
                return basketItems;
            }
        }
        return new List<BasketItem>();
    }

    public async Task Remove(int id)
    {
        if (_httpContextAccessor.HttpContext.User.Identity.IsAuthenticated)
        {
            AppUser appUser = await _userManager.FindByNameAsync(_httpContextAccessor.HttpContext.User.Identity.Name);
            Basket? basket = await _context.Baskets.Include(x => x.BasketItems.Where(y => !y.IsDeleted))
                .Where(x => !x.IsDeleted && x.AppUserId == appUser.Id).FirstOrDefaultAsync();
            BasketItem basketItem = await _context.BasketItems.Where(x=>!x.IsDeleted && x.BasketId==basket.Id && x.ProductId==id).FirstOrDefaultAsync();
            if (basketItem != null)
            {
                basketItem.IsDeleted = true;
                await _context.SaveChangesAsync();
            }
        }
        else
        {
            var cookiesBasket = _httpContextAccessor.HttpContext.Request.Cookies["basket"];
            if (cookiesBasket != null)
            {
                List<Basket>? baskets = JsonConvert.DeserializeObject<List<Basket>>(cookiesBasket);
                Basket basket = baskets.FirstOrDefault(x=>x.Id == id);
                if (basket is not null)
                {
                    baskets.Remove(basket);
                    cookiesBasket = JsonConvert.SerializeObject(baskets);
                    _httpContextAccessor.HttpContext.Response.Cookies.Append("basket", cookiesBasket);
                }
            }
            
        }
    }
}