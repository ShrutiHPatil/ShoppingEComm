using Microsoft.EntityFrameworkCore;

namespace ECommSiteApis.Models
{
    public class ApplicationDbContext : DbContext
    {
        // public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options) { }
        public DbSet<User> Users { get; set; }
        public DbSet<Products> Products { get; set; }
        public DbSet<CartItems> CartItems { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    modelBuilder.Entity<User>().HasKey(u => u.UserId);
        //    modelBuilder.Entity<Products>().HasKey(p => p.ProductId);
        //    modelBuilder.Entity<CartItems>().HasKey(ci => ci.CartItemId);

        //    modelBuilder.Entity<CartItems>()
        //        .HasOne(ci => ci.User)
        //        .WithMany(u => u.CartItems)
        //        .HasForeignKey(ci => ci.UserId);

        //    modelBuilder.Entity<CartItems>()
        //        .HasOne(ci => ci.Product)
        //        .WithMany(p => p.CartItems)
        //        .HasForeignKey(ci => ci.ProductId);

        //    base.OnModelCreating(modelBuilder);
        //}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CartItems>()
                .HasKey(ci => new { ci.UserId, ci.ProductId });

            modelBuilder.Entity<CartItems>()
                .HasOne(ci => ci.User)
                .WithMany(u => u.CartItems)
                .HasForeignKey(ci => ci.UserId);

            modelBuilder.Entity<CartItems>()
                .HasOne(ci => ci.Product)
                .WithMany(p => p.CartItems)
                .HasForeignKey(ci => ci.ProductId);

            base.OnModelCreating(modelBuilder);

        }
    }
}
