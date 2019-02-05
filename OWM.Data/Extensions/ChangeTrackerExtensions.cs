using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using OWM.Domain.Entities;

namespace OWM.Data.Extensions
{
    public static class ChangeTrackerExtensions
    {
        public static void ApplyAuditInformation(this ChangeTracker changeTracker)
        {
            foreach (var entry in changeTracker.Entries())
            {
                if (!(entry.Entity is BaseAuditClass baseAudit)) continue;

                var now = DateTime.UtcNow;
                switch (entry.State)
                {
                    case EntityState.Added:
                        baseAudit.Created = now;
                        baseAudit.Modified = now;
                        break;
                    case EntityState.Modified:
                        baseAudit.Modified = now;
                        break;
                }
            }
        }
    }
}