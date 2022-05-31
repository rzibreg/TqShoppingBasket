using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using Tq.ShoppingBasket.Application.Validators;
using Tq.ShoppingBasket.Domain;
using Tq.ShoppingBasket.Domain.Repositories;

namespace Tq.ShoppingBasket.Application.Service
{
    public class ShoppingBasketService : IShoppingBasketService
    {
        private readonly ILogger<ShoppingBasketService> _logger;
        private readonly IShoppingBasketRepository _shoppingBasketRepository;
        private readonly IShoppingBasketValidator _shoppingBasketValidator;

        private Domain.ShoppingBasket Basket { get; set; }


        public ShoppingBasketService(IShoppingBasketRepository shoppingBasketRepository, IShoppingBasketValidator shoppingBasketValidator, ILogger<ShoppingBasketService> logger = null)
        {
            _shoppingBasketRepository = shoppingBasketRepository;
            _shoppingBasketValidator = shoppingBasketValidator;
            _logger = logger;

            Basket = new Domain.ShoppingBasket
            {
                Products = new List<Product>(),
                Discounts = new List<Discount>(),
                Sum = 0M,
                Results = new ProcessResult
                {
                    Total = 0M,
                    Discount = 0M,
                    BasketId = Guid.NewGuid()
                }
            };
        }

        public ProcessResult ProcessBasket(List<Product> products, List<Discount> discounts)
        {
            AddProduct(products);
            AddDiscount(discounts);
            Process();
            Save();

            return Basket.Results;
        }

        private void AddProduct(Product product)
        {
            Basket.Products.Add(product);

            _logger?.LogInformation("Product {product} added", product.Name);
        }

        private void AddProduct(List<Product> products)
        {
            foreach (var product in products)
                AddProduct(product);
        }

        private void AddDiscount(Discount discount)
        {
            if (_shoppingBasketValidator.IsDiscountAlreadyPresent(Basket.Discounts, discount.Name)) return;

            Basket.Discounts.Add(discount);

            _logger?.LogInformation("Discount {name} added", discount.Name);
        }

        private void AddDiscount(List<Discount> discounts)
        {
            foreach (var discount in discounts)
                AddDiscount(discount);
        }

        private void Process()
        {
            CalculateSum();
            CalculateDiscount();
            CalculateTotal();
            LogProcess();
        }

        private void CalculateSum()
        {
            if (_shoppingBasketValidator.BasketContainsProducts(Basket))
                Basket.Sum = Basket.Products.Sum(x => x.Price);
        }

        private void CalculateDiscount()
        {
            if (_shoppingBasketValidator.IsDiscountPossible(Basket))
                Basket.Results.Discount = Basket.Discounts.Sum(discount => discount.CalculateDiscount(Basket.Products));
        }

        private void CalculateTotal()
        {
            Basket.Results.Total = Basket.Sum - Basket.Results.Discount;
        }

        private void LogProcess()
        {
            _logger?.Log(LogLevel.Information, "Guid: {guid}, Total: {sum}, Discount: {discount}",
                Basket.Results.BasketId.ToString(), Basket.Results.Total.ToString("F"), Basket.Results.Discount.ToString("F"));
        }

        private void Save()
        {
            _shoppingBasketRepository.AddToDataStore(Basket);
        }
    }
}
