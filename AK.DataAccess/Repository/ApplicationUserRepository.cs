﻿using AK.DataAccess.Data;
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
    public class ApplicationUserRepository : Repository<ApplicationUser>, IApplicationUserRepository
    {
        private ApplicationDbContext _db;
        public ApplicationUserRepository(ApplicationDbContext db):base(db)
        {
            _db = db;
        }


        public async Task<ApplicationUser> GetFirstOrDefaultAsync(Expression<Func<ApplicationUser, bool>> filter)
        {
            return await _db.ApplicationUsers.FirstOrDefaultAsync(filter);
        }
        public void Update(ApplicationUser applicationUser)
        {
            _db.ApplicationUsers.Update(applicationUser);
        }

    }
}
