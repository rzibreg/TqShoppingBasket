using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tq.ShoppingBasket.Domain.Repositories
{
    public interface IRepository<T>
    {
        Task<IEnumerable<T>> GetAll();
    }
}
