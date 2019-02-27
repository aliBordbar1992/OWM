using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using OWM.Domain.Entities;
using OWM.Domain.Entities.Enums;

namespace OWM.Data.Configurations
{
    public class TeamConfig : IEntityTypeConfiguration<Team>
    {
        public void Configure(EntityTypeBuilder<Team> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.AgeRange)
                .HasConversion(new EnumToNumberConverter<AgeRange, int>());
        }
    }
}