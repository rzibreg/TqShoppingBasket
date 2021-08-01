using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tq.ShoppingBasket.Models;

namespace Tq.ShoppingBasket.Interfaces
{
    public interface IProduct
    {
        string Name { get; set; }
        decimal Price { get; set; }
        ProductCategory Category { get; set; }
    }
}
