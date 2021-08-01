using System.Collections.Generic;

namespace Tq.ShoppingBasket.Interfaces
{
    public interface IShoppingBasket
    {
        List<IProduct> Products { get; set; }
        List<IDiscount> Discounts { get; set; }
        decimal Sum { get; set; }
        decimal Discount { get; set; }
        decimal Total { get; set; }
        void AddProduct(IProduct product);
        void AddProduct(List<IProduct> products);
        void AddDiscount(IDiscount discount);
        void AddDiscount(List<IDiscount> discounts);
        void Calculate();
    }
}
