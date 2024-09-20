using AK.DataAccess.Data;
using AK.DataAccess.Repository.IRepository;
using AK.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AK.DataAccess.Repository
{
    public class GalleryRepository : Repository<Gallery>,IGalleryRepository
    {
        private ApplicationDbContext _db;

        public GalleryRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(Gallery obj)
        {
            _db.Gallerys.Update(obj);
        }
        public async Task<IEnumerable<Gallery>> GetAllAsync()
        {
            return await _db.Gallerys.ToListAsync(); // Assuming _db.Galleries is your DbSet
        }

    }
}
