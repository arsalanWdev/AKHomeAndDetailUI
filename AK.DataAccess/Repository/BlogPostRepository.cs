using AK.DataAccess.Data;
using AK.DataAccess.Repository.IRepository;
using AK.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AK.DataAccess.Repository
{
    public class BlogPostRepository:Repository<BlogPost>,IBlogPostRepository
    {
        private readonly ApplicationDbContext _db;

        public BlogPostRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(BlogPost obj)
        {
            _db.BlogPosts.Update(obj); // Assuming BlogPosts is your DbSet
        }

        public IEnumerable<BlogPost> GetAll()
        {
            return _db.BlogPosts.ToList(); // Fetch all blog posts
        }
    }
}
