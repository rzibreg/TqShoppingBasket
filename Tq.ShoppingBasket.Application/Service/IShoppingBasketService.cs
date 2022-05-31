using System.Collections.Generic;
using Tq.ShoppingBasket.Domain;

namespace Tq.ShoppingBasket.Application.Service
{
    public interface IShoppingBasketService
    {
        ProcessResult ProcessBasket(List<Product> products, List<Discount> discounts);
    }
}
