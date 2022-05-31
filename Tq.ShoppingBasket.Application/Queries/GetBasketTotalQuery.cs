using MediatR;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Tq.ShoppingBasket.Application.ViewModels;
using Tq.ShoppingBasket.Domain;

namespace Tq.ShoppingBasket.Application.Queries
{
    public class GetBasketTotalQuery : IRequest<BasketViewModel>
    {
        [Required]
        public List<Product> Products { get; set; }

        [Required]
        public List<Discount> Discounts { get; set; }
    }
}
