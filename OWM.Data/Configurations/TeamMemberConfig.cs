using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OWM.Domain.Entities;

namespace OWM.Data.Configurations
{
    public class TeamMemberConfig : IEntityTypeConfiguration<TeamMember>
    {
        public void Configure(EntityTypeBuilder<TeamMember> builder)
        {
            builder.ToTable("TeamMembers");
            builder.HasKey(tm => new
            {
                tm.TeamId,
                tm.ProfileId
            });
            builder.Ignore(x => x.Id);
            builder
                .HasOne(t => t.Team)
                .WithMany(m => m.Members)
                .HasForeignKey(tm => tm.TeamId);
            builder
                .HasOne(tm => tm.MemberProfile)
                .WithMany(t => t.Teams)
                .HasForeignKey(p => p.ProfileId);
        }
    }
}