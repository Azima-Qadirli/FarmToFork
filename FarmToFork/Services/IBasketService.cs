using System.Diagnostics;
using FarmToFork.Models;

namespace FarmToFork.Services;

public interface IBasketService
{
    public Task AddBasket(int id, int? count);
    public Task<List<BasketItem>> GetAll();
    public Task Remove(int id);
}