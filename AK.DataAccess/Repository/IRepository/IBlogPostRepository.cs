using AK.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AK.DataAccess.Repository.IRepository
{
    public interface IBlogPostRepository : IRepository<BlogPost>
    {
        void Update(BlogPost obj);
        IEnumerable<BlogPost> GetAll();
    }
}
