using System.Collections.Generic;
using System.Threading.Tasks;
using Tq.ShoppingBasket.Domain.Repositories;

namespace Tq.ShoppingBasket.Infrastructure
{
    public class ShoppingBasketRepository : IShoppingBasketRepository
    {
        private static List<Domain.ShoppingBasket> _products;

        public ShoppingBasketRepository()
        {
            _products = new List<Domain.ShoppingBasket>();
        }

        public async Task<IEnumerable<Domain.ShoppingBasket>> GetAll()
        {
            return await Task.FromResult(_products);
        }

        public async Task AddToDataStore(Domain.ShoppingBasket basket)
        {
            _products.Add(basket);
            await Task.CompletedTask;
        }
    }
}
