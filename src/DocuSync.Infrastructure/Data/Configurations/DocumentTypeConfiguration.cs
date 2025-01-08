using DocuSync.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace DocuSync.Infrastructure.Data.Configurations
{
    public class DocumentTypeConfiguration : IEntityTypeConfiguration<DocumentType>
    {
        public void Configure(EntityTypeBuilder<DocumentType> builder)
        {
            builder.ToTable("DocumentTypes");

            builder.HasKey(d => d.Id);

            builder.Property(d => d.Name)
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(d => d.Description)
                .HasMaxLength(500);

            builder.Property(d => d.Frequency)
                .HasConversion<string>()
                .HasMaxLength(20)
                .IsRequired();

            // Audit fields
            builder.Property(d => d.CreatedAt)
                .IsRequired();
            builder.Property(d => d.CreatedBy)
                .HasMaxLength(256)
                .IsRequired();
            builder.Property(d => d.LastModifiedAt);
            builder.Property(d => d.LastModifiedBy)
                .HasMaxLength(256);
        }
    }
}