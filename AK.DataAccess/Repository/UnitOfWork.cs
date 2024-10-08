﻿using AK.DataAccess.Data;
using AK.DataAccess.Repository.IRepository;
using AK.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AK.DataAccess.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private ApplicationDbContext _db;
        public ICategoryRepository Category { get; private set; }
        public IProductRepository Product { get; private set; }
        public IShoppingCartRepository ShoppingCart { get; private set; }
        public IApplicationUserRepository ApplicationUser { get; private set; }
        public IOrderHeaderRepository OrderHeader { get; private set; }
        public IOrderDetailRepository OrderDetail { get; private set; }
        public IPortfolioRepository Portfolio { get; private set; }
        public IGalleryRepository Gallery { get; private set; }
        public IFavouriteRepository Favourite { get; private set; }
             public IBlogPostRepository BlogPost { get; private set; }
        public IRepository<IdentityUserRole<string>> UserRole { get; private set; }
        public IRepository<IdentityRole> Role { get; private set; }
        public IConsultationRequestRepository ConsultationRequest { get; private set; }
        public UnitOfWork(ApplicationDbContext db)
        {
            _db = db;
            ApplicationUser = new ApplicationUserRepository(_db);
            ShoppingCart = new ShoppingCartRepository(_db);
            Category = new CategoryRepository(_db);
            Product = new ProductRepository(_db);
            OrderHeader = new OrderHeaderRepository(_db);
            OrderDetail = new OrderDetailRepository(_db);
            Portfolio = new PortfolioRepository(_db);
           Gallery  = new GalleryRepository(_db);
            Favourite= new FavouriteRepository(_db);
            BlogPost    = new BlogPostRepository(_db);
            UserRole = new Repository<IdentityUserRole<string>>(_db);
            Role = new Repository<IdentityRole>(_db);
            ConsultationRequest = new ConsultationRequestRepository(_db);
        }

        public void ClearChangeTracker()
        {
            foreach (var entry in _db.ChangeTracker.Entries().ToList())
            {
                entry.State = EntityState.Detached;
            }
        }


        public void Save()
        {
            _db.SaveChanges();
        }
        public async Task SaveAsync()
        {
            await _db.SaveChangesAsync();
        }
    }
}
