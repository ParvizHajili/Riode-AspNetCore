using Microsoft.EntityFrameworkCore;
using Riode.WebUI.Models.Entities;

namespace Riode.WebUI.Models.DataContexts
{
    public class RiodeDbContext : DbContext
    {
        public RiodeDbContext() : base()
        {

        }

        public RiodeDbContext(DbContextOptions options) : base(options)
        {

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Data Source=Localhost;Initial Catalog=RiodeDb;Integrated Security=true;");
            }
        }

        public DbSet<Contact> Contacts { get; set; }
        public DbSet<Brand> Brands { get; set; }
        public DbSet<ProductColor> ProductColors { get; set; }
        public DbSet<ProductSize> ProductSizes { get; set; }
        public DbSet<Category> Categories { get; set; }
    }
}
