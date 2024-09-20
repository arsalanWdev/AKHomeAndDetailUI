using AK.DataAccess.Data;
using AK.DataAccess.Repository.IRepository;
using AK.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace AK.DataAccess.Repository
{
    public class FavouriteRepository : Repository<Favourite>, IFavouriteRepository
    {
        private readonly ApplicationDbContext _db;

        public FavouriteRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
        public async Task<Favourite> GetFirstOrDefaultAsync(Expression<Func<Favourite, bool>> filter)
        {
            return await _db.Favourites.Include(f => f.Gallery) // Include related gallery
                                       .FirstOrDefaultAsync(filter);
        }

        public async Task<IEnumerable<Favourite>> GetFavouritesByUserAsync(string userId)
        {
            return await _db.Favourites
                            .Include(f => f.Gallery) // Include related Gallery
                            .Where(f => f.UserId == userId)
                            .ToListAsync();
        }

    }

}
