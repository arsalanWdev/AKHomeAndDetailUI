using AK.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AK.DataAccess.Data
{
    public class ApplicationDbContext : IdentityDbContext<IdentityUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ShoppingCart> ShoppingCarts { get; set; }

        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<OrderHeader> OrderHeaders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<Portfolio> Portfolios { get; set; }
        public DbSet<Gallery> Gallerys { get; set; }
        public DbSet<Favourite> Favourites { get; set; }
        public DbSet<BlogPost> BlogPosts { get; set; }
        public DbSet<ConsultationRequest> ConsultationRequests { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);


            

            modelBuilder.Entity<Favourite>()
          .HasOne(f => f.Gallery)
          .WithMany()
          .HasForeignKey(f => f.GalleryId);

            modelBuilder.Entity<Favourite>()
                .HasOne(f => f.User)
                .WithMany()
                .HasForeignKey(f => f.UserId);
             

            // Seeding Categories
            modelBuilder.Entity<Category>().HasData(
                new Category { Id = 1, Name = "Furniture", DisplayOrder = 1 },
                new Category { Id = 2, Name = "Lighting", DisplayOrder = 2 },
                new Category { Id = 3, Name = "Rugs", DisplayOrder = 3 },
                new Category { Id = 4, Name = "Curtains", DisplayOrder = 4 },
                new Category { Id = 5, Name = "Wall Art", DisplayOrder = 5 },
                new Category { Id = 6, Name = "Decorative Accessories", DisplayOrder = 6 }
            );


            // Seeding Products
            modelBuilder.Entity<Product>().HasData(
                new Product
                {
                    Id = 1,
                    Title = "Sofa Set",
                    Author = "HomeStyle",
                    Description = "Elegant and comfortable sofa set for your living room.",
                    SKU = "SOFA123",
                    ListPrice = 799.00,
                    Price = 750.00,
                    Price50 = 725.00,
                    Price100 = 700.00,
                    CategoryId = 1,
                    ImageUrl = ""  // Add the image URL here if available
                },
                new Product
                {
                    Id = 2,
                    Title = "Chandelier",
                    Author = "Lumiere",
                    Description = "Crystal chandelier to add a touch of luxury to any room.",
                    SKU = "CHAN456",
                    ListPrice = 399.00,
                    Price = 375.00,
                    Price50 = 365.00,
                    Price100 = 350.00,
                    CategoryId = 2,
                    ImageUrl = ""  // Add the image URL here if available
                },
                new Product
                {
                    Id = 3,
                    Title = "Persian Rug",
                    Author = "Oriental Rugs",
                    Description = "Handcrafted Persian rug with intricate designs.",
                    SKU = "RUG789",
                    ListPrice = 499.00,
                    Price = 475.00,
                    Price50 = 460.00,
                    Price100 = 450.00,
                    CategoryId = 3,
                    ImageUrl = ""  // Add the image URL here if available
                },
                new Product
                {
                    Id = 4,
                    Title = "Silk Curtains",
                    Author = "Elegance",
                    Description = "Luxurious silk curtains to enhance the beauty of your windows.",
                    SKU = "CURT101",
                    ListPrice = 129.00,
                    Price = 120.00,
                    Price50 = 115.00,
                    Price100 = 110.00,
                    CategoryId = 4,
                    ImageUrl = ""  // Add the image URL here if available
                },
                new Product
                {
                    Id = 5,
                    Title = "Modern Art Canvas",
                    Author = "ArtHouse",
                    Description = "Abstract modern art canvas to brighten up your walls.",
                    SKU = "ART202",
                    ListPrice = 199.00,
                    Price = 180.00,
                    Price50 = 170.00,
                    Price100 = 160.00,
                    CategoryId = 5,
                    ImageUrl = ""  // Add the image URL here if available
                },
                new Product
                {
                    Id = 6,
                    Title = "Decorative Vases",
                    Author = "DecorElite",
                    Description = "Set of decorative vases for stylish arrangements.",
                    SKU = "VASE303",
                    ListPrice = 89.00,
                    Price = 85.00,
                    Price50 = 80.00,
                    Price100 = 75.00,
                    CategoryId = 6,
                    ImageUrl = ""  // Add the image URL here if available
                }
            );
        }
    }
}
