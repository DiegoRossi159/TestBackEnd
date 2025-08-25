namespace TestBackEnd.src.Data.Config
{
    public class RentalConfiguration : IEntityTypeConfiguration<Rental>
    {
        public void Configure(EntityTypeBuilder<Rental> builder)
        {
            builder.ToTable("Rentals");

            builder.HasKey(r => r.RentalId);

            builder.Property(r => r.RentalId)
                .ValueGeneratedOnAdd()
                .IsRequired();

            builder.Property(r => r.StartDate)
                .IsRequired();

            builder.Property(r => r.EndDate)
                .IsRequired();

            builder.Property(r => r.ExpectedEndDate)
                .IsRequired();

            builder.HasOne(r => r.Motorcycle)
                .WithMany(m => m.Rentals)
                .HasForeignKey(r => r.MotorcycleId);

            builder.HasOne(r => r.Deliverer)
                .WithMany(d => d.Rentals)
                .HasForeignKey(r => r.DelivererId);

            builder.HasOne(r => r.RentalType)
                .WithMany(rt => rt.Rentals)
                .HasForeignKey(r => r.RentalTypeId);
        }
    }
}
