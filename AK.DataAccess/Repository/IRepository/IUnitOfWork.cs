using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AK.DataAccess.Repository.IRepository
{
    public interface IUnitOfWork
    {
        ICategoryRepository Category { get; }
        IProductRepository Product { get; }

        IShoppingCartRepository ShoppingCart { get; }
        IApplicationUserRepository ApplicationUser { get; }
        IOrderDetailRepository OrderDetail { get; }
        IOrderHeaderRepository OrderHeader { get; }
        IPortfolioRepository Portfolio { get; }
        IGalleryRepository Gallery { get; }
        IFavouriteRepository Favourite { get; }
        IBlogPostRepository BlogPost { get; }
        IRepository<IdentityUserRole<string>> UserRole { get; }
        IRepository<IdentityRole> Role { get; }

        IConsultationRequestRepository ConsultationRequest { get; }
        void ClearChangeTracker();
        void Save();
        Task SaveAsync(); // Async save method

    }
}
