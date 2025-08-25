namespace TestBackEnd.src.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }
        public DbSet<Deliverer> Deliverers { get; set; }
        public DbSet<RentalType> RentalTypes { get; set; }

        public DbSet<Motorcycle> Motorcycles { get; set; }
        public DbSet<Rental> Rentals { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new DelivererConfiguration());
            modelBuilder.ApplyConfiguration(new MotorcycleConfiguration());
            modelBuilder.ApplyConfiguration(new RentalConfiguration());
            modelBuilder.ApplyConfiguration(new RentalTypeConfiguration());

            modelBuilder.Entity<RentalType>().HasData(
                new RentalType { RentalTypeId = "1", Days = 7, Cost = 30.00m },
                new RentalType { RentalTypeId = "2", Days = 15, Cost = 28.00m },
                new RentalType { RentalTypeId = "3", Days = 30, Cost = 22.00m },
                new RentalType { RentalTypeId = "4", Days = 45, Cost = 20.00m },
                new RentalType { RentalTypeId = "5", Days = 50, Cost = 18.00m }
            );
        }
    }
}
