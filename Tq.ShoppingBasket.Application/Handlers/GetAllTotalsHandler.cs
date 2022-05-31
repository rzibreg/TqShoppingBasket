using AutoMapper;
using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Tq.ShoppingBasket.Application.Queries;
using Tq.ShoppingBasket.Application.ViewModels;
using Tq.ShoppingBasket.Domain.Repositories;

namespace Tq.ShoppingBasket.Application.Handlers
{
    public class GetAllTotalsHandler : IRequestHandler<GetAllTotalsQuery, List<BasketViewModel>>
    {
        private readonly IShoppingBasketRepository _shoppingBasketRepository;
        private readonly IMapper _mapper;

        public GetAllTotalsHandler(IShoppingBasketRepository shoppingBasketRepository, IMapper mapper)
        {
            _shoppingBasketRepository = shoppingBasketRepository;
            _mapper = mapper;
        }

        public Task<List<BasketViewModel>> Handle(GetAllTotalsQuery request, CancellationToken cancellationToken)
        {
            var basket = _shoppingBasketRepository.GetAll();

            return Task.FromResult(_mapper.Map<List<BasketViewModel>>(basket.Result.Select(x => x.Results)));
        }
    }
}
