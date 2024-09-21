using AK.Models;
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace AK.DataAccess.Repository.IRepository
{
    public interface IConsultationRequestRepository : IRepository<ConsultationRequest>
    {
        Task<IEnumerable<ConsultationRequest>> GetAllAsync(Expression<Func<ConsultationRequest, bool>> filter);

        Task<ConsultationRequest> GetFirstOrDefaultAsync(Expression<Func<ConsultationRequest, bool>> filter, string? includeProperties = null, bool tracked = false);
        void Update(ConsultationRequest consultationRequest);
    }
}
