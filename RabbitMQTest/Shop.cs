using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitMQTest
{
    internal class Shop
    {
        private List<Product> products = new List<Product>();
        private int nextId = 1;

        public Shop()
        {
            AddProduct("Balón de fútbol", 25.99f);
            AddProduct("Raqueta de tenis", 45.00f);
            AddProduct("Guantes de boxeo", 30.50f);
            AddProduct("Zapatillas deportivas", 60.00f);
        }

        public void AddProduct(string nombre, float precio)
        {
            products.Add(new Product(nextId++, nombre, precio));
        }

        public bool DeleteProduct(int id)
        {
            var product = products.FirstOrDefault(p => p.Id == id);
            if (product != null)
            {
                products.Remove(product);
                return true;
            }
            return false;
        }

        public List<Product> GetProducts() => products;

        public Product? GetProductsByID(int id) => products.FirstOrDefault(p => p.Id == id);
    }
}
