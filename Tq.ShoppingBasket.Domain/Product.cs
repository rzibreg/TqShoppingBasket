using Tq.ShoppingBasket.Domain.Enums;

namespace Tq.ShoppingBasket.Domain
{
    public class Product
    {
        public Product(string name, decimal price, ProductCategory category)
        {
            Name = name;
            Price = price;
            Category = category;
        }

        public string Name { get; set; }
        public decimal Price { get; set; }
        public ProductCategory Category { get; set; }
    }
}
