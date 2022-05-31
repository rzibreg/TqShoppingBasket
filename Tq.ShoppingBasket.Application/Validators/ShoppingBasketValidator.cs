using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;
using Tq.ShoppingBasket.Domain;

namespace Tq.ShoppingBasket.Application.Validators
{
    public class ShoppingBasketValidator : IShoppingBasketValidator
    {
        private readonly ILogger<IShoppingBasketValidator> _logger;

        public ShoppingBasketValidator(ILogger<IShoppingBasketValidator> logger)
        {
            _logger = logger;
        }

        public bool IsDiscountAlreadyPresent(List<Discount> allDiscounts, string newDiscount)
        {
            var validationResult = allDiscounts.Any(x => x.Name == newDiscount);
            
            if(validationResult)
                _logger.LogInformation("Discount '{discount}' present, skipping", newDiscount);

            return validationResult;
        }

        public bool IsDiscountPossible(Domain.ShoppingBasket shoppingBasket)
        {
            return shoppingBasket.Discounts.Any() && shoppingBasket.Products.Any();
        }

        public bool BasketContainsProducts(Domain.ShoppingBasket shoppingBasket)
        {
            return shoppingBasket.Products.Any();
        }
    }
}
