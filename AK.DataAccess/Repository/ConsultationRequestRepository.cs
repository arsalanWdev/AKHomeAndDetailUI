    using AK.DataAccess.Data;
    using AK.DataAccess.Repository.IRepository;
    using AK.Models;
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading.Tasks;

    namespace AK.DataAccess.Repository
    {
        public class ConsultationRequestRepository : Repository<ConsultationRequest>, IConsultationRequestRepository
        {
            private readonly ApplicationDbContext _db;

            public ConsultationRequestRepository(ApplicationDbContext db) : base(db)
            {
                _db = db;
            }

        public async Task<IEnumerable<ConsultationRequest>> GetAllAsync(Expression<Func<ConsultationRequest, bool>> filter = null)
        {
            IQueryable<ConsultationRequest> query = _db.ConsultationRequests;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            return await query.ToListAsync();
        }




        public async Task<ConsultationRequest> GetFirstOrDefaultAsync(Expression<Func<ConsultationRequest, bool>> filter, string? includeProperties = null, bool tracked = false)
            {
                IQueryable<ConsultationRequest> query = tracked ? _db.ConsultationRequests : _db.ConsultationRequests.AsNoTracking();
                query = query.Where(filter);

                if (!string.IsNullOrEmpty(includeProperties))
                {
                    foreach (var includeProp in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                    {
                        query = query.Include(includeProp);
                    }
                }

                return await query.FirstOrDefaultAsync();
            }

            public void Update(ConsultationRequest consultationRequest)
            {
                var objFromDb = _db.ConsultationRequests.FirstOrDefault(c => c.Id == consultationRequest.Id);
                if (objFromDb != null)
                {
                    // Update properties
                    objFromDb.Name = consultationRequest.Name;
                    objFromDb.Email = consultationRequest.Email;
                    objFromDb.Message = consultationRequest.Message;
                    objFromDb.IsApproved = consultationRequest.IsApproved; 
                    objFromDb.DateRequested = consultationRequest.DateRequested;

                    // Mark the entity as modified
                    _db.Entry(objFromDb).State = EntityState.Modified;
                }
            }
        }
    }
