using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Riode.WebUI.Models.Entities;
using Riode.WebUI.Models.Membership;

namespace Riode.WebUI.Models.DataContexts
{
    public class RiodeDbContext :IdentityDbContext<RiodeUser, RiodeRole, int, RiodeUserClaim, RiodeUserRole, RiodeUserLogin, RiodeRoleClaim, RiodeUserToken>
    {
        public RiodeDbContext(DbContextOptions options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<RiodeUser>(e =>
            {
                e.ToTable("Users", "Membership");
            });

            builder.Entity<RiodeRole>(e =>
            {
                e.ToTable("Roles", "Membership");
            });

            builder.Entity<RiodeUserRole>(e =>
            {
                e.ToTable("UserRoles", "Membership");
            });

            builder.Entity<RiodeUserClaim>(e =>
            {
                e.ToTable("UserClaims", "Membership");
            });

            builder.Entity<RiodeRoleClaim>(e =>
            {
                e.ToTable("RoleClaim", "Membership");
            });

            builder.Entity<RiodeUserLogin>(e =>
            {
                e.ToTable("UserLogins", "Membership");
            });

            builder.Entity<RiodeUserToken>(e =>
            {
                e.ToTable("UserTokens", "Membership");
            });
        }

        public DbSet<Contact> Contacts { get; set; }
        public DbSet<Brand> Brands { get; set; }
        public DbSet<ProductColor> Colors { get; set; }
        public DbSet<ProductSize> Sizes { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductImage> ProductImages { get; set; }
        public DbSet<ProductSizeColorItem> ProductSizeColorColection { get; set; }
    }
}
