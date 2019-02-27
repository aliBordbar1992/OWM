using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
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

        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<UserClaim> UserClaims { get; set; }
        public virtual DbSet<UserLogin> UserLogins { get; set; }
        public virtual DbSet<UserToken> UserTokens { get; set; }

        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<RoleClaim> RoleClaims { get; set; }
        public virtual DbSet<UserRole> UserRoles { get; set; }

        public virtual DbSet<Profile> Profiles { get; set; }
        public virtual DbSet<City> Cities { get; set; }
        public virtual DbSet<Country> Countries { get; set; }
        public virtual DbSet<Ethnicity> Ethnicities { get; set; }
        public virtual DbSet<Occupation> Occupations { get; set; }
        public virtual DbSet<Interest> Interests { get; set; }

        public virtual DbSet<Team> Teams { get; set; }
        public virtual DbSet<MilesPledged> MilesPledged { get; set; }
        public virtual DbSet<CompletedMiles> CompletedMiles { get; set; }




        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(@"Server=.;Database=Owm;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserClaim>().HasKey(p => new { p.Id });
            modelBuilder.Entity<UserLogin>().HasKey(p => new { p.LoginProvider, p.ProviderKey });
            modelBuilder.Entity<UserToken>().HasKey(p => new { p.UserId, p.LoginProvider, p.Name });
            modelBuilder.Entity<RoleClaim>().HasKey(p => new { p.Id });

            modelBuilder.ApplyConfiguration(new UserRoleConfig());
            modelBuilder.ApplyConfiguration(new ProfileConfig());
            modelBuilder.ApplyConfiguration(new TeamConfig());

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
