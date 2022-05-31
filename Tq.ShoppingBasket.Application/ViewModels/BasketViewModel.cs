using System;

namespace Tq.ShoppingBasket.Application.ViewModels
{
    public class BasketViewModel
    {
        public Guid BasketId { get; set; }
        public decimal Total { get; set; }
        public decimal Discount { get; set; }
    }
}
