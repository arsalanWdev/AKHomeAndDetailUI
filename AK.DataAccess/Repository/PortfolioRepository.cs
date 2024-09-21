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
        public class PortfolioRepository : Repository<Portfolio>, IPortfolioRepository
        {
            private ApplicationDbContext _db;

            public PortfolioRepository(ApplicationDbContext db) : base(db)
            {
                _db = db;
            }

            public void Update(Portfolio obj)
            {
                _db.Portfolios.Update(obj);
            }

            // Method to get portfolios by UserId
            public IEnumerable<Portfolio> GetPortfolioByDesignerId(string userId)
            {
                return _db.Portfolios
                          .Where(p => p.UserId == userId)
                          .ToList();
            }
        }
    }
