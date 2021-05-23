using Microsoft.EntityFrameworkCore;
using DSModels;

namespace DSDL
{
    public class FannerDogsDBContext: DbContext
    {
        public FannerDogsDBContext(DbContextOptions options) : base(options)
        { 
        }
        protected FannerDogsDBContext()
        {

        }
        public DbSet<StoreLocation> StoreLocations { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Inventory> Inventories { get; set; }
        public DbSet<ManagesStore> ManagesStores { get; set; }
        public DbSet<DogManager> DogManagers { get; set; }
        public DbSet<DogBuyer> DogBuyers { get; set; }
        public DbSet<DogOrder> DogOrders { get; set; }
        public DbSet<Dog> Dogs { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Dog>()
                .Property(dog => dog.Id)
                .ValueGeneratedOnAdd();
            modelBuilder.Entity<DogOrder>()
                .Property(dogOrder => dogOrder.Id)
                .ValueGeneratedOnAdd();
            modelBuilder.Entity<ManagesStore>()
                .Property(mS => mS.Id)
                .ValueGeneratedOnAdd();
            modelBuilder.Entity<Inventory>()
                .Property(inv => inv.Id)
                .ValueGeneratedOnAdd();
            modelBuilder.Entity<OrderItem>()
                .Property(ordIt => ordIt.Id)
                .ValueGeneratedOnAdd();
            modelBuilder.Entity<DogManager>()
                .HasKey(dM => dM.PhoneNumber);
            modelBuilder.Entity<DogBuyer>()
                .HasKey(dogB => dogB.PhoneNumber);
        }
    }
}