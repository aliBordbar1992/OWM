using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using OWM.Domain.Entities;
using OWM.Domain.Entities.Enums;

namespace OWM.Data.Configurations
{
    public class ProfileConfig : IEntityTypeConfiguration<Profile>
    {
        public void Configure(EntityTypeBuilder<Profile> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Gender)
                .HasConversion(new EnumToNumberConverter<GenderEnum, int>());
        }
    }
}
