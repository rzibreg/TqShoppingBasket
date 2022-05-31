using System;
using System.Collections.Generic;
using System.Linq;
using Tq.ShoppingBasket.Domain.Constants;
using Tq.ShoppingBasket.Domain.Enums;
using Tq.ShoppingBasket.Domain.Exceptions;

namespace Tq.ShoppingBasket.Domain
{
    public class Discount
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
        private decimal TotalDiscount { get; set; }
        private int NumberOfDiscounts { get; set; }
        public decimal CalculateDiscount(List<Product> products)
        {
            try
            {
                if (products == null || !products.Any())
                    return TotalDiscount;

                GetNumberOfDiscounts(products);
                var discountedProduct = products.FirstOrDefault(x => x.Category == DiscountedCategory);

                if (discountedProduct == null)
                    return TotalDiscount;

                var reduction = GetReduction(discountedProduct);
                TotalDiscount = NumberOfDiscounts * reduction;

                return TotalDiscount;
            }
            catch (DomainException e)
            {
                throw new DomainException(e.Message, e);
            }
        }

        private void GetNumberOfDiscounts(List<Product> products)
        {
            NumberOfDiscounts = Convert.ToInt32(Math.Floor(products.Count(x => x.Category == RequiredCategory) / RequiredQuantity));
        }

        private decimal GetReduction(Product discountedProduct)
        {
            var reduction = Math.Round(discountedProduct.Price * (DiscountPercent / DiscountConstants.HundredPercent), DiscountConstants.TwoDecimalPlaces);
            return reduction;
        }
    }
}
