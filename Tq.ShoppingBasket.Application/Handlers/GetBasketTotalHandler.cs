using MediatR;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Tq.ShoppingBasket.Application.Queries;
using Tq.ShoppingBasket.Application.Service;
using Tq.ShoppingBasket.Application.ViewModels;

namespace Tq.ShoppingBasket.Application.Handlers
{
    public class GetBasketTotalHandler : IRequestHandler<GetBasketTotalQuery, BasketViewModel>
    {
        private readonly IShoppingBasketService _shoppingBasketService;
        private readonly IMapper _mapper;

        public GetBasketTotalHandler(IShoppingBasketService shoppingBasketService, IMapper mapper)
        {
            _shoppingBasketService = shoppingBasketService;
            _mapper = mapper;
        }

        public Task<BasketViewModel> Handle(GetBasketTotalQuery request, CancellationToken cancellationToken)
        {
            var result = _shoppingBasketService.ProcessBasket(request.Products, request.Discounts);

            return Task.FromResult(_mapper.Map<BasketViewModel>(result));
        }
    }
}
