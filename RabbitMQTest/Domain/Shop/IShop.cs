using RabbitMQTest.Domain.Models;

namespace RabbitMQTest.Domain.Shop;

public interface IShop
{
    public List<Product> Products { get; }
}