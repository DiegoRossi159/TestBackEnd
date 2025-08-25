namespace TestBackEnd.src.Data.Config
{
    public class RentalTypeConfiguration : IEntityTypeConfiguration<RentalType>
    {
        public void Configure(EntityTypeBuilder<RentalType> builder)
        {
            builder.ToTable("RentalTypes");

            builder.HasKey(rt => rt.RentalTypeId);

            builder.Property(rt => rt.Days)
                .IsRequired();

            builder.Property(rt => rt.Cost)
                .IsRequired();
        }
    }
}
