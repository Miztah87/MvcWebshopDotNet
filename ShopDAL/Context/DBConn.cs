using ShopDAL.DomainModel;
using System.Data.Entity;

namespace ShopDAL.Context
{
    public class DbConn: DbContext
    {
        public DbConn() : base("DefaultConnection")
        {
            Database.SetInitializer(new DbInitializer());
        }

        public DbSet<Category> categories { get; set; }
        public DbSet<Product> products { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<Cart> Carts { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>()
                .HasMany(p => p.Products)
                 .WithMany(c => c.Categories);
        }

    }
}
