using AK.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace AK.DataAccess.Repository.IRepository
{
    public interface IFavouriteRepository : IRepository<Favourite>
    {
        Task<Favourite> GetFirstOrDefaultAsync(Expression<Func<Favourite, bool>> filter);
        Task<IEnumerable<Favourite>> GetFavouritesByUserAsync(string userId);
    }

}
