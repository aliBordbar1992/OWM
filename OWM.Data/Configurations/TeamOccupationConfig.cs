using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OWM.Domain.Entities;

namespace OWM.Data.Configurations
{
    public class TeamOccupationConfig : IEntityTypeConfiguration<TeamOccupations>
    {
        public void Configure(EntityTypeBuilder<TeamOccupations> builder)
        {
            builder.HasKey(bc => new
            {
                bc.TeamId,
                bc.OccupationId
            });
            builder.Ignore(x => x.Id);
            builder
                .HasOne(bc => bc.Team)
                .WithMany(b => b.AllowedOccupations)
                .HasForeignKey(bc => bc.TeamId);
            builder
                .HasOne(bc => bc.Occupation)
                .WithMany(c => c.InTeams)
                .HasForeignKey(bc => bc.OccupationId);
        }
    }
}