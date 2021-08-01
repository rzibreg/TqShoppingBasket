using System;
using System.Collections.Generic;
using System.Linq;
using Tq.ShoppingBasket.Interfaces;

namespace Tq.ShoppingBasket.Models
{
    public class Discount : IDiscount
    {
        public Discount(string name, ProductCategory requiredCategory, decimal requiredQuantity, ProductCategory discountedCategory, decimal discountPercent)
        {
            Name = name;
            RequiredCategory = requiredCategory;
            RequiredQuantity = requiredQuantity;
            DiscountedCategory = discountedCategory;
            DiscountPercent = discountPercent;
            TotalDiscount = 0M;
        }

        public string Name { get; set; }
        public ProductCategory RequiredCategory { get; set; }
        public decimal RequiredQuantity { get; set; }
        public ProductCategory DiscountedCategory { get; set; }
        public decimal DiscountPercent { get; set; }
        public decimal TotalDiscount { get; set; }
        public int NumberOfDiscounts { get; set; }
        public decimal CalculateDiscount(List<IProduct> products)
        {
            if (products == null || !products.Any()) 
                return TotalDiscount;

            NumberOfDiscounts = Convert.ToInt32(Math.Floor(products.Count(x => x.Category == RequiredCategory) / RequiredQuantity));
            var discountedProduct = products.FirstOrDefault(x => x.Category == DiscountedCategory);

            if (discountedProduct == null) 
                return TotalDiscount;

            var reduction = Math.Round(discountedProduct.Price * (DiscountPercent / 100), 2);
            TotalDiscount = NumberOfDiscounts * reduction;

            return TotalDiscount;
        }
    }
}
