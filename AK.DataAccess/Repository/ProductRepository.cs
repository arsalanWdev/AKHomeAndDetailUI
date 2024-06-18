using AK.DataAccess.Data;
using AK.DataAccess.Repository.IRepository;
using AK.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace AK.DataAccess.Repository
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        private ApplicationDbContext _db;
        public ProductRepository(ApplicationDbContext db):base(db)
        {
            _db = db;
        }
     

        public void Update(Product obj)
        {
            var objfromdb = _db.Products.FirstOrDefault(u => u.Id == obj.Id);
            if(objfromdb != null)
            {
                objfromdb.Title = obj.Title;
                objfromdb.ISBN = obj.ISBN;
              
                objfromdb.Price = obj.Price;
                objfromdb.Price50 = obj.Price50;
                objfromdb.ListPrice= obj.ListPrice;
                objfromdb.Price100 = obj.Price100;
                objfromdb.Description = obj.Description;
                objfromdb.CategoryId = obj.CategoryId;
                objfromdb.Author = obj.Author;
                if(obj.ImageUrl != null)
                {
                    objfromdb.ImageUrl = obj.ImageUrl;
                }


            }

        }
    }
}
