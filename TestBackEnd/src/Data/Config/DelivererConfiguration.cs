namespace TestBackEnd.src.Data.Config
{
    public class DelivererConfiguration : IEntityTypeConfiguration<Deliverer>
    {
        public void Configure(EntityTypeBuilder<Deliverer> builder)
        {
            builder.ToTable("Deliverer");

            builder.HasKey(d => d.DelivererId);

            builder.Property(d => d.CNPJ)
                .IsRequired()
                .HasMaxLength(14);

            builder.HasIndex(d => d.CNPJ)
                .IsUnique();

            builder.Property(d => d.CNH)
                .IsRequired()
                .HasMaxLength(11);

            builder.HasIndex(d => d.CNH)
                .IsUnique();

            builder.Property(d => d.CNHImgPath)
                .IsRequired(false);


            builder.HasMany(d => d.Rentals)
                .WithOne(r => r.Deliverer)
                .HasForeignKey(r => r.DelivererId);
        }
    }
}
