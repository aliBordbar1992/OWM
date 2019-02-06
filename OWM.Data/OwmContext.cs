using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using OWM.Data.Configurations;
using OWM.Data.Extensions;
using OWM.Domain.Entities;

namespace OWM.Data
{
    public class OwmContext : DbContext
    {
        public OwmContext(DbContextOptions options) : base(options)
        {
        }

        public virtual DbSet<City> Cities { get; set; }
        public virtual DbSet<Country> Countries { get; set; }
        public virtual DbSet<Ethnicity> Ethnicities { get; set; }
        public virtual DbSet<Occupation> Occupations { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<UserIdentity> Identities { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(@"Server=.;Database=Owm;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new UserConfig());
            modelBuilder.ApplyConfiguration(new UserIdentityConfig());

            base.OnModelCreating(modelBuilder);
        }

        public override async Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            ChangeTracker.ApplyAuditInformation();
            return await base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }
    }
}
