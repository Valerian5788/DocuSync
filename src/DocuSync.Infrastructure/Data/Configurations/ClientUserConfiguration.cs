using DocuSync.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocuSync.Infrastructure.Data.Configurations
{
    public class ClientUserConfiguration : IEntityTypeConfiguration<ClientUser>
    {
        public void Configure(EntityTypeBuilder<ClientUser> builder)
        {
            builder.ToTable("ClientUsers");

            builder.HasKey(cu => cu.Id);

            builder.Property(cu => cu.AzureAdUserId)
                .HasMaxLength(256)
                .IsRequired();

            builder.HasOne(cu => cu.Client)
                .WithMany()
                .HasForeignKey(cu => cu.ClientId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(cu => new { cu.ClientId, cu.AzureAdUserId })
                .IsUnique();
        }
    }
}
