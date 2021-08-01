using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using Tq.ShoppingBasket.Interfaces;

namespace Tq.ShoppingBasket.Business
{
    public class ShoppingBasket : IShoppingBasket
    {
        public List<IProduct> Products { get; set; }
        public List<IDiscount> Discounts { get; set; }
        public decimal Sum { get; set; }
        public decimal Discount { get; set; }
        public decimal Total { get; set; }

        private readonly ILogger<ShoppingBasket> _logger;

        public ShoppingBasket(ILogger<ShoppingBasket> logger = null)
        {
            Products = new List<IProduct>();
            Discounts = new List<IDiscount>();
            Sum = 0M;
            Discount = 0M;
            Total = 0M;

            _logger = logger;
        }

        public void AddProduct(IProduct product)
        {
            Products.Add(product);

            _logger?.LogInformation("Product {product} added.", product.Name);
        }

        public void AddProduct(List<IProduct> products)
        {
            foreach (var product in products)
                AddProduct(product);
        }

        public void AddDiscount(IDiscount discount)
        {
            if (Discounts.Any(x => x.Name == discount.Name)) return;
            
            Discounts.Add(discount);

            _logger?.LogInformation("Discount {name} added.", discount.Name);
        }

        public void AddDiscount(List<IDiscount> discounts)
        {
            foreach (var discount in discounts)
                AddDiscount(discount);
        }

        public void Calculate()
        {
            CalculateSum();
            CalculateDiscount();
            CalculateTotal();

            _logger?.Log(LogLevel.Information, "Sum: {sum}", Sum.ToString("F"));
            _logger?.Log(LogLevel.Information, "Discount: {discount}", Discount.ToString("F"));
            _logger?.Log(LogLevel.Information, "Total: {total}", Total.ToString("F"));
            _logger?.Log(LogLevel.Information, "Following discounts were applied: {discounts}",
                string.Join("; ", Discounts.Select(x => $"{x.NumberOfDiscounts}x {x.Name}")));
        }

        private void CalculateSum()
        {
            if(Products.Any())
                Sum = Products.Sum(x => x.Price);
        }

        private void CalculateDiscount()
        {
            if (Discounts.Any() && Products.Any())
                Discount = Discounts.Sum(discount => discount.CalculateDiscount(Products));
        }

        private void CalculateTotal()
        {
            Total = Sum - Discount;
        }
    }
}
