using Microsoft.EntityFrameworkCore;
using Volo.Abp;
using Volo.Abp.EntityFrameworkCore.Modeling;
using Alberta.ServiceDesk.Facilities;
using Alberta.ServiceDesk.Bookings;
using Alberta.ServiceDesk.Policies;

namespace Alberta.ServiceDesk.EntityFrameworkCore;

public static class ServiceDeskModelBuilderExtensions
{
    public static void ConfigureServiceDesk(this ModelBuilder builder)
    {
        Check.NotNull(builder, nameof(builder));

        builder.Entity<Facility>(b =>
        {
            b.ToTable("facilities");
            b.ConfigureByConvention();
            b.Property(x => x.Name).IsRequired().HasMaxLength(128);
            b.Property(x => x.Type).HasMaxLength(64);
            b.Property(x => x.OwnerUnit).HasMaxLength(64);
            b.Property(x => x.Description).HasMaxLength(512);

            b.HasIndex(x => new { x.OwnerUnit, x.Type });
        });

        builder.Entity<Booking>(b =>
        {
            b.ToTable("bookings");
            b.ConfigureByConvention();
            b.Property(x => x.Purpose).IsRequired().HasMaxLength(256);

            b.HasIndex(x => new { x.FacilityId, x.StartTime });
        });

        builder.Entity<BookingApproval>(b =>
        {
            b.ToTable("booking_approvals");
            b.ConfigureByConvention();
        });

        builder.Entity<PolicyRule>(b =>
        {
            b.ToTable("policy_rules");
            b.ConfigureByConvention();
        });
    }
}
