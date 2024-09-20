using AK.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AK.DataAccess.Repository.IRepository
{
    public interface IGalleryRepository:IRepository<Gallery>
    {
        void Update(Gallery obj);
        Task<IEnumerable<Gallery>> GetAllAsync();

    }
}
