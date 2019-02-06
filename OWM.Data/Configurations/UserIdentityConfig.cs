using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using OWM.Domain.Entities;
using OWM.Domain.Entities.Enums;

namespace OWM.Data.Configurations
{
    public class UserIdentityConfig : IEntityTypeConfiguration<UserIdentity>
    {
        public void Configure(EntityTypeBuilder<UserIdentity> builder)
        {
            builder.HasKey(x => x.Id);
        }
    }
}