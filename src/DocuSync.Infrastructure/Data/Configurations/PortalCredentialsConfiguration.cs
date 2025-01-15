using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DocuSync.Domain.Entities;

namespace DocuSync.Infrastructure.Data.Configurations
{
    public class PortalCredentialsConfiguration : IEntityTypeConfiguration<PortalCredentials>
    {
        public void Configure(EntityTypeBuilder<PortalCredentials> builder)
        {
            builder.ToTable("PortalCredentials");

            builder.HasKey(p => p.Id);

            builder.Property(p => p.PortalType)
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(p => p.Username)
                .HasMaxLength(256)
                .IsRequired();

            builder.Property(p => p.EncryptedPassword)
                .IsRequired();

            builder.HasOne(p => p.Client)
                .WithMany()
                .HasForeignKey(p => p.ClientId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
