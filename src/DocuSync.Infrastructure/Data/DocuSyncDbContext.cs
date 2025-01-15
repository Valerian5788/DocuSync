using DocuSync.Domain.Entities;
using DocuSync.Infrastructure.Data.Configurations;
using Microsoft.EntityFrameworkCore;

namespace DocuSync.Infrastructure.Data
{
    public class DocuSyncDbContext : DbContext
    {
        public DbSet<Client> Clients { get; set; }
        public DbSet<DocumentType> DocumentTypes { get; set; }
        public DbSet<Requirement> Requirements { get; set; }
        public DbSet<ClientUser> ClientUsers { get; set; }
        public DbSet<PortalCredentials> PortalCredentials { get; set; }

        public DocuSyncDbContext(DbContextOptions<DocuSyncDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new ClientUserConfiguration());
            modelBuilder.ApplyConfiguration(new ClientConfiguration());
            modelBuilder.ApplyConfiguration(new DocumentTypeConfiguration());
            modelBuilder.ApplyConfiguration(new RequirementConfiguration());
            modelBuilder.ApplyConfiguration(new PortalCredentialsConfiguration());
        }
    }
}