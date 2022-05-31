using System.Threading.Tasks;

namespace Tq.ShoppingBasket.Domain.Repositories
{
    public interface IShoppingBasketRepository : IRepository<ShoppingBasket>
    {
        Task AddToDataStore(ShoppingBasket basket);
    }
}
