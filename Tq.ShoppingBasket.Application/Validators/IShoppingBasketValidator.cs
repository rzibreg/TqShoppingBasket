using System.Collections.Generic;
using Tq.ShoppingBasket.Domain;

namespace Tq.ShoppingBasket.Application.Validators
{
    public interface IShoppingBasketValidator
    {
        bool IsDiscountAlreadyPresent(List<Discount> allDiscounts, string newDiscount);
        bool IsDiscountPossible(Domain.ShoppingBasket shoppingBasket);
        bool BasketContainsProducts(Domain.ShoppingBasket shoppingBasket);

        // Additional validation...
    }
}