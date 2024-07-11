using AK.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AK.DataAccess.Data
{
    public class ApplicationDbContext : IdentityDbContext<IdentityUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options):base(options)
        {

        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ShoppingCart> ShoppingCarts { get; set; }
        public DbSet<Company> Companies { get; set; }

        public DbSet<ApplicationUser> ApplicationUsers { get; set; }

        public DbSet<OrderHeader> OrderHeaders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Category>().HasData(
       new Category { Id = 1, Name = "Software Development", DisplayOrder = 1 },
       new Category { Id = 2, Name = "Algorithms", DisplayOrder = 2 },
       new Category { Id = 3, Name = "Interview Preparation", DisplayOrder = 3 }
   );

            modelBuilder.Entity<Company>().HasData(
              new Company 
              { Id = 1, 
                  Name = "Tech Solutions",
                  StreetAddress = "123 Tech St",
                  City="Tech City",
                  PostalCode= "12121",
                  State="IL",
                  PhoneNumber="91221842179" },
              new Company
              {
                  Id = 2,
                  Name = "Vivid Books",
                  StreetAddress = "123 vivid St",
                  City = "Vivid City",
                  PostalCode = "6666",
                  State = "IL",
                  PhoneNumber = "394273847"
              },

                 new Company
                 {
                     Id = 3,
                     Name = "Readers Club",
                     StreetAddress = "123 Main St",
                     City = "Readers City",
                     PostalCode = "3444",
                     State = "IL",
                     PhoneNumber = "4234234"
                 }

              );


            modelBuilder.Entity<Product>().HasData(
     new Product
     {
         Id = 1,
         Title = "Clean Code: A Handbook of Agile Software Craftsmanship",
         Author = "Robert C. Martin",
         Description = "A book about writing cleaner, more efficient code.",
         ISBN = "978-0132350884",
         ListPrice = 40.00,
         Price = 35.00,
         Price50 = 33.00,
         Price100 = 30.00,
         CategoryId = 1,
         ImageUrl = ""
     },
     new Product
     {
         Id = 2,
         Title = "The Pragmatic Programmer: Your Journey to Mastery",
         Author = "Andrew Hunt, David Thomas",
         Description = "A book about software engineering best practices.",
         ISBN = "978-0135957059",
         ListPrice = 50.00,
         Price = 45.00,
         Price50 = 42.00,
         Price100 = 40.00,
         CategoryId = 1,
         ImageUrl = ""
     },
     new Product
     {
         Id = 3,
         Title = "Design Patterns: Elements of Reusable Object-Oriented Software",
         Author = "Erich Gamma, Richard Helm, Ralph Johnson, John Vlissides",
         Description = "A book about common design patterns in software development.",
         ISBN = "978-0201633610",
         ListPrice = 60.00,
         Price = 55.00,
         Price50 = 52.00,
         Price100 = 50.00,
         CategoryId = 1,
         ImageUrl = ""
     },
     new Product
     {
         Id = 4,
         Title = "Introduction to Algorithms",
         Author = "Thomas H. Cormen, Charles E. Leiserson, Ronald L. Rivest, Clifford Stein",
         Description = "A comprehensive book on algorithms.",
         ISBN = "978-0262033848",
         ListPrice = 90.00,
         Price = 85.00,
         Price50 = 82.00,
         Price100 = 80.00,
         CategoryId = 2,
         ImageUrl = ""
     },
     new Product
     {
         Id = 5,
         Title = "You Don't Know JS: Scope & Closures",
         Author = "Kyle Simpson",
         Description = "A book about JavaScript scope and closures.",
         ISBN = "978-1449335588",
         ListPrice = 35.00,
         Price = 30.00,
         Price50 = 28.00,
         Price100 = 25.00,
         CategoryId = 1,
         ImageUrl = ""
     },
     new Product
     {
         Id = 6,
         Title = "The Art of Computer Programming, Volumes 1-4A Boxed Set",
         Author = "Donald E. Knuth",
         Description = "A comprehensive series of books on computer programming.",
         ISBN = "978-0321751041",
         ListPrice = 240.00,
         Price = 220.00,
         Price50 = 210.00,
         Price100 = 200.00,
         CategoryId = 2,
         ImageUrl = ""
     },
     new Product
     {
         Id = 7,
         Title = "Cracking the Coding Interview: 189 Programming Questions and Solutions",
         Author = "Gayle Laakmann McDowell",
         Description = "A book to help software developers prepare for coding interviews.",
         ISBN = "978-0984782857",
         ListPrice = 40.00,
         Price = 35.00,
         Price50 = 33.00,
         Price100 = 30.00,
         CategoryId = 3,
         ImageUrl = ""
     },
     new Product
     {
         Id = 8,
         Title = "Refactoring: Improving the Design of Existing Code",
         Author = "Martin Fowler",
         Description = "A book about improving the design of existing code.",
         ISBN = "978-0201485677",
         ListPrice = 55.00,
         Price = 50.00,
         Price50 = 48.00,
         Price100 = 45.00,
         CategoryId = 1,
         ImageUrl = ""
     },
     new Product
     {
         Id = 9,
         Title = "JavaScript: The Good Parts",
         Author = "Douglas Crockford",
         Description = "A book about the best features of JavaScript.",
         ISBN = "978-0596517748",
         ListPrice = 30.00,
         Price = 28.00,
         Price50 = 25.00,
         Price100 = 20.00,
         CategoryId = 1,
         ImageUrl = ""
     }
     );

        }
    }
    }

