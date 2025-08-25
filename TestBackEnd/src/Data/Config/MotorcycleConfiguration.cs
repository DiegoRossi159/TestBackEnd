namespace TestBackEnd.src.Data.Config
{
    public class MotorcycleConfiguration : IEntityTypeConfiguration<Motorcycle>
    {
        public void Configure(EntityTypeBuilder<Motorcycle> builder)
        {
            builder.ToTable("Motorcycles");

            builder.HasKey(m => m.MotorcycleId);

            builder.Property(m => m.LicensePlate)
                .IsRequired()
                .HasMaxLength(10);

            builder.HasIndex(m => m.LicensePlate)
                .IsUnique();

            builder.HasMany(m => m.Rentals)
                .WithOne(r => r.Motorcycle)
                .HasForeignKey(r => r.MotorcycleId);
        }
    }
}
