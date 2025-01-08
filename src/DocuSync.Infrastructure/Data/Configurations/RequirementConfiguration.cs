using DocuSync.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace DocuSync.Infrastructure.Data.Configurations
{
    public class RequirementConfiguration : IEntityTypeConfiguration<Requirement>
    {
        public void Configure(EntityTypeBuilder<Requirement> builder)
        {
            builder.ToTable("Requirements");

            builder.HasKey(r => r.Id);

            builder.Property(r => r.DueDate)
                .IsRequired();

            builder.Property(r => r.Status)
                .HasConversion<string>()
                .HasMaxLength(20)
                .IsRequired();

            // Relationships
            builder.HasOne<Client>()
                .WithMany()
                .HasForeignKey(r => r.ClientId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne<DocumentType>()
                .WithMany()
                .HasForeignKey(r => r.DocumentTypeId)
                .OnDelete(DeleteBehavior.Restrict);

            // Audit fields
            builder.Property(r => r.CreatedAt)
                .IsRequired();
            builder.Property(r => r.CreatedBy)
                .HasMaxLength(256)
                .IsRequired();
            builder.Property(r => r.LastModifiedAt);
            builder.Property(r => r.LastModifiedBy)
                .HasMaxLength(256);
        }
    }
}