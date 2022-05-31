using Microsoft.Extensions.Logging;
using Moq;
using System.Collections.Generic;
using System.Linq;
using Tq.ShoppingBasket.Application.Service;
using Tq.ShoppingBasket.Application.Validators;
using Tq.ShoppingBasket.Domain;
using Tq.ShoppingBasket.Domain.Enums;
using Tq.ShoppingBasket.Domain.Repositories;
using Tq.ShoppingBasket.Tests.Extensions;
using Xunit;

namespace Tq.ShoppingBasket.Tests
{
    public class ShoppingBasketServiceTests
    {
        private readonly Mock<ILogger<ShoppingBasketService>> _loggerMock = new();
        private readonly Mock<IShoppingBasketRepository> _shoppingBasketRepositoryMock = new();
        private readonly Mock<IShoppingBasketValidator> _shoppingBasketValidatorMock = new();
        private readonly ShoppingBasketService _shoppingBasketService;

        public ShoppingBasketServiceTests()
        {
            _shoppingBasketService = new ShoppingBasketService(_shoppingBasketRepositoryMock.Object, 
                                                                _shoppingBasketValidatorMock.Object, 
                                                                _loggerMock.Object);
        }

        [Fact]
        public void Logging_is_called_on_process_basket_with_single_product_and_single_discount()
        {
            // Arrange
            var products = new List<Product> { Butter() };
            var discounts = new List<Discount>{ GetBuyTwoButtersGetOneBreadForHalfPriceDiscount() };

            // Act
            var processResult = _shoppingBasketService.ProcessBasket(products, discounts);

            // Assert
            _loggerMock
                .VerifyLogging($"Product {Butter().Name} added")
                .VerifyLogging($"Discount {GetBuyTwoButtersGetOneBreadForHalfPriceDiscount().Name} added")
                .VerifyLogging($"Guid: {processResult.BasketId}, Total: {processResult.Total:F}, Discount: {processResult.Discount:F}");
        }

        [Fact]
        public void ProcessBasket_calculates_total_without_any_discounts()
        {
            // Arrange
            _shoppingBasketValidatorMock.Setup(x => x.BasketContainsProducts(It.IsAny<Domain.ShoppingBasket>())).Returns(true);
            var products = new List<Product>
            {
                Butter(),
                Bread(),
                Milk()
            };

            // Act
            var processResult = _shoppingBasketService.ProcessBasket(products, NoDiscounts());

            // Assert
            Assert.Equal(products.Sum(x => x.Price), processResult.Total);
        }

        [Fact]
        public void ProcessBasket_calculates_total_and_discount_with_discounts_set()
        {
            // Arrange
            _shoppingBasketValidatorMock.Setup(x => x.BasketContainsProducts(It.IsAny<Domain.ShoppingBasket>())).Returns(true);
            _shoppingBasketValidatorMock.Setup(x => x.IsDiscountAlreadyPresent(It.IsAny<List<Discount>>(), It.IsAny<string>())).Returns(false);
            _shoppingBasketValidatorMock.Setup(x => x.IsDiscountPossible(It.IsAny<Domain.ShoppingBasket>())).Returns(true);

            var products = new List<Product>
            {
                Butter(),
                Butter(),
                Bread(),
                Bread()
            };
            var discounts = GetStandardDiscounts();

            // Act
            var processResult = _shoppingBasketService.ProcessBasket(products, discounts);

            // Assert
            Assert.Equal(3.1M, processResult.Total);
            Assert.Equal(0.5M, processResult.Discount);
        }

        // More tests here...

        #region TestHelperMethods

        private Product Butter()
        {
            return new Product("Butter", 0.8M, ProductCategory.Butter);
        }

        private Product Milk()
        {
            return new Product("Milk", 1.15M, ProductCategory.Milk);
        }

        private Product Bread()
        {
            return new Product("Bread", 1M, ProductCategory.Bread);
        }

        private List<Discount> GetStandardDiscounts()
        {
            return new List<Discount>
            {
                GetBuyTwoButtersGetOneBreadForHalfPriceDiscount(),
                GetBuyThreeMilksGetFourthForFree()
            };
        }

        private List<Discount> NoDiscounts()
        {
            return new List<Discount>();
        }

        private Discount GetBuyTwoButtersGetOneBreadForHalfPriceDiscount()
        {
            return new Discount("Buy two butters and get one bread at 50% off.",
                ProductCategory.Butter,
                2,
                ProductCategory.Bread,
                50);
        }

        private Discount GetBuyThreeMilksGetFourthForFree()
        {
            return new Discount("Buy three milks and get the fourth milk for free.",
                ProductCategory.Milk,
                4,
                ProductCategory.Milk,
                100);
        }

        #endregion
    }
}
