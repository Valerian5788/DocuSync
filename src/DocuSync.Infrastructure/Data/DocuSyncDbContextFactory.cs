using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace DocuSync.Infrastructure.Data
{
    public class DocuSyncDbContextFactory : IDesignTimeDbContextFactory<DocuSyncDbContext>
    {
        public DocuSyncDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<DocuSyncDbContext>();
            optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=DocuSync;Trusted_Connection=True;MultipleActiveResultSets=true");

            return new DocuSyncDbContext(optionsBuilder.Options);
        }
    }
}