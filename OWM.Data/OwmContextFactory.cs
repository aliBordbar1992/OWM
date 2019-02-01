using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace OWM.Data
{
    public class OwmContextFactory : IDesignTimeDbContextFactory<OwmContext>
    {
        private static string DataConnectionString => new DatabaseConfiguration().GetDataConnectionString();

        public OwmContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<OwmContext>();
            optionsBuilder.UseSqlServer(DataConnectionString);
            return new OwmContext(optionsBuilder.Options);
        }
    }
}