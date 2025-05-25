using RabbitMQTest.Domain.Models;

namespace RabbitMQTest.Domain.Shop
{
    internal class Shop : IShop
    {
        public List<Product> Products { get; } = [];

        public Shop()
        {
            AddProduct("Balón de fútbol", 25.99f);
            AddProduct("Raqueta de tenis", 45.00f);
            AddProduct("Guantes de boxeo", 30.50f);
            AddProduct("Zapatillas deportivas", 60.00f);
        }

        public void AddProduct(string nombre, float precio)
        {
            Products.Add(new Product(Products.Count + 1, nombre, precio));
        }

        public bool DeleteProduct(int id)
        {
            var product = Products.FirstOrDefault(p => p.Id == id);
            if (product != null)
            {
                Products.Remove(product);
                return true;
            }
            return false;
        }

        public Product? GetProductByID(int id) => Products.FirstOrDefault(p => p.Id == id);
    }
}
