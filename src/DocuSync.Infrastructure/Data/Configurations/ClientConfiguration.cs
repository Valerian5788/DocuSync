using DocuSync.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace DocuSync.Infrastructure.Data.Configurations
{
    public class ClientConfiguration : IEntityTypeConfiguration<Client>
    {
        public void Configure(EntityTypeBuilder<Client> builder)
        {
            builder.ToTable("Clients");

            builder.HasKey(c => c.Id);

            builder.Property(c => c.Name)
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(c => c.HorusEmail)
                .HasMaxLength(256)
                .IsRequired();

            builder.Property(c => c.Status)
                .HasConversion<string>()
                .HasMaxLength(20)
                .IsRequired();

            // Audit fields
            builder.Property(c => c.CreatedAt)
                .IsRequired();
            builder.Property(c => c.CreatedBy)
                .HasMaxLength(256)
                .IsRequired();
            builder.Property(c => c.LastModifiedAt);
            builder.Property(c => c.LastModifiedBy)
                .HasMaxLength(256);
        }
    }
}