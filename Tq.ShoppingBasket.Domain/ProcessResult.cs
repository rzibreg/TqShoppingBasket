using System;

namespace Tq.ShoppingBasket.Domain
{
    public class ProcessResult
    {
        public Guid BasketId { get; set; }
        public decimal Total { get; set; }
        public decimal Discount { get; set; }
    }
}
