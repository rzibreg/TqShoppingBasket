using Microsoft.Extensions.Logging;
using Moq;
using System.Collections.Generic;
using System.Linq;
using Tq.ShoppingBasket.Interfaces;
using Tq.ShoppingBasket.Models;
using Xunit;

namespace Tq.ShoppingBasket.Tests
{
    public class ShoppingBasketTests
    {
        private readonly Mock<ILogger<Business.ShoppingBasket>> _loggerMock = new();

        [Fact]
        public void AddProduct_adds_single_product_to_basket()
        {
            var basket = new Business.ShoppingBasket(_loggerMock.Object);

            basket.AddProduct(Butter());

            Assert.Single(basket.Products);
            _loggerMock.VerifyInformationLogWasCalled($"Product {Butter().Name} added.");
        }

        [Fact]
        public void AddProduct_adds_multiple_products_to_basket()
        {
            var basket = new Business.ShoppingBasket(_loggerMock.Object);
            var products = new List<IProduct>
            {
                Butter(),
                Bread(),
                Milk()
            };

            basket.AddProduct(products);

            Assert.Equal(products.Count, basket.Products.Count);
            _loggerMock
                .VerifyLogging($"Product {Butter().Name} added.")
                .VerifyLogging($"Product {Bread().Name} added.")
                .VerifyLogging($"Product {Milk().Name} added.");
        }

        [Fact]
        public void AddDiscount_adds_single_discount_to_basket()
        {
            var basket = new Business.ShoppingBasket(_loggerMock.Object);

            basket.AddDiscount(GetBuyTwoButtersGetOneBreadForHalfPriceDiscount());

            Assert.Single(basket.Discounts);
            _loggerMock.VerifyInformationLogWasCalled($"Discount {GetBuyTwoButtersGetOneBreadForHalfPriceDiscount().Name} added.");
        }

        [Fact]
        public void AddDiscount_adds_multiple_discounts_to_basket()
        {
            var basket = new Business.ShoppingBasket(_loggerMock.Object);

            basket.AddDiscount(GetStandardDiscounts());

            Assert.Equal(GetStandardDiscounts().Count, basket.Discounts.Count);
            _loggerMock
                .VerifyLogging($"Discount {GetBuyTwoButtersGetOneBreadForHalfPriceDiscount().Name} added.")
                .VerifyLogging($"Discount {GetBuyThreeMilksGetFourthForFree().Name} added.");
        }

        /// <summary>
        /// Given the basket has 1 bread, 1 butter and 1 milk 
        /// when I total the basket 
        /// then the total should be 2.95
        /// </summary>
        [Fact]
        public void Calculate_sets_product_sum_without_any_discounts()
        {
            var basket = new Business.ShoppingBasket(_loggerMock.Object);
            var products = new List<IProduct>
            {
                Butter(),
                Bread(),
                Milk()
            };

            basket.AddProduct(products);
            basket.Calculate();

            Assert.Equal(products.Sum(x => x.Price), basket.Sum);
            _loggerMock.VerifyInformationLogWasCalled($"Sum: {products.Sum(x => x.Price):F}");
        }

        /// <summary>
        /// Given the basket has 2 butter and 2 bread 
        /// when I total the basket 
        /// then the total should be 3.10
        /// </summary>
        [Fact]
        public void Calculate_sets_product_sum_according_to_second_scenario()
        {
            var basket = new Business.ShoppingBasket(_loggerMock.Object);
            var products = new List<IProduct>
            {
                Butter(),
                Butter(),
                Bread(),
                Bread()
            };
            basket.AddProduct(products);
            basket.AddDiscount(GetStandardDiscounts());

            basket.Calculate();

            Assert.Equal(3.1M, basket.Total);
            Assert.Equal(0.5M, basket.Discount);
            _loggerMock
                .VerifyLogging($"Discount: {basket.Discount:F}")
                .VerifyLogging($"Total: {basket.Total:F}");
        }

        /// <summary>
        /// Given the basket has 4 milks
        /// when I total the basket
        /// then the total should be 3.45
        /// </summary>
        [Fact]
        public void Calculate_sets_product_sum_according_to_third_scenario()
        {
            var basket = new Business.ShoppingBasket(_loggerMock.Object);
            var products = new List<IProduct>
            {
                Milk(),
                Milk(),
                Milk(),
                Milk()
            };
            basket.AddProduct(products);
            basket.AddDiscount(GetStandardDiscounts());

            basket.Calculate();

            Assert.Equal(3.45M, basket.Total);
            Assert.Equal(1.15M, basket.Discount);

            _loggerMock
                .VerifyLogging($"Discount: {basket.Discount:F}")
                .VerifyLogging($"Total: {basket.Total:F}");
        }

        /// <summary>
        /// Given the basket has 2 butter, 1 bread and 8 milk 
        /// when I total the basket 
        /// then the total should be 9.00
        /// </summary>
        [Fact]
        public void Calculate_sets_product_sum_according_to_fourth_scenario()
        {
            var basket = new Business.ShoppingBasket(_loggerMock.Object);
            var products = new List<IProduct>
            {
                Butter(),
                Butter(),
                Bread(),
                Milk(),
                Milk(),
                Milk(),
                Milk(),
                Milk(),
                Milk(),
                Milk(),
                Milk()
            };
            basket.AddProduct(products);
            basket.AddDiscount(GetStandardDiscounts());

            basket.Calculate();

            Assert.Equal(9M, basket.Total);
            Assert.Equal(2.8M, basket.Discount);
            Assert.Equal(1, basket.Discounts.First().NumberOfDiscounts);
            Assert.Equal(2, basket.Discounts.Last().NumberOfDiscounts);
            _loggerMock
                .VerifyLogging($"Discount: {basket.Discount:F}")
                .VerifyLogging($"Total: {basket.Total:F}")
                .VerifyLogging($"Following discounts were applied: {string.Join("; ", basket.Discounts.Select(x => $"{x.NumberOfDiscounts}x {x.Name}"))}");
        }

        #region TestHelperMethods

        private Product Butter()
        {
            return new("Butter", 0.8M, ProductCategory.Butter);
        }

        private Product Milk()
        {
            return new("Milk", 1.15M, ProductCategory.Milk);
        }

        private Product Bread()
        {
            return new("Bread", 1M, ProductCategory.Bread);
        }

        private List<IDiscount> GetStandardDiscounts()
        {
            return new()
            {
                GetBuyTwoButtersGetOneBreadForHalfPriceDiscount(),
                GetBuyThreeMilksGetFourthForFree()
            };
        }

        private Discount GetBuyTwoButtersGetOneBreadForHalfPriceDiscount()
        {
            return new("Buy two butters and get one bread at 50% off.",
                ProductCategory.Butter,
                2,
                ProductCategory.Bread,
                50);
        }

        private Discount GetBuyThreeMilksGetFourthForFree()
        {
            return new("Buy three milks and get the fourth milk for free.",
                ProductCategory.Milk,
                4,
                ProductCategory.Milk,
                100);
        }

        #endregion
    }
}
