using MediatR;
using System.Collections.Generic;
using Tq.ShoppingBasket.Application.ViewModels;

namespace Tq.ShoppingBasket.Application.Queries
{
    public class GetAllTotalsQuery : IRequest<List<BasketViewModel>>
    {
    }
}
