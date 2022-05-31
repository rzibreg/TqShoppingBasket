using System.Collections.Generic;

namespace Tq.ShoppingBasket.Domain
{
    public class ShoppingBasket
    {
        public List<Product> Products { get; set; }
        public List<Discount> Discounts { get; set; }
        public decimal Sum { get; set; }
        public ProcessResult Results  { get; set; }
    }
}
