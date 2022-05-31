using AutoMapper;
using Tq.ShoppingBasket.Application.ViewModels;
using Tq.ShoppingBasket.Domain;

namespace Tq.ShoppingBasket.Application.Mappers
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<BasketViewModel, ProcessResult>().ReverseMap();
        }
    }
}
