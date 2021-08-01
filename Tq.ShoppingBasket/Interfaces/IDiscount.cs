using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tq.ShoppingBasket.Models;

namespace Tq.ShoppingBasket.Interfaces
{
    public interface IDiscount
    {
        string Name { get; set; }
        ProductCategory RequiredCategory { get; set; }
        decimal RequiredQuantity { get; set; }
        ProductCategory DiscountedCategory { get; set; }
        decimal DiscountPercent { get; set; }
        decimal TotalDiscount { get; set; }
        int NumberOfDiscounts { get; set; }

        decimal CalculateDiscount(List<IProduct> products);
    }
}
