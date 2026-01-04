using Microsoft.EntityFrameworkCore;
using Volo.Abp.AuditLogging.EntityFrameworkCore;
using Volo.Abp.BackgroundJobs.EntityFrameworkCore;
using Volo.Abp.BlobStoring.Database.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Modeling;
using Volo.Abp.FeatureManagement.EntityFrameworkCore;
using Volo.Abp.Identity;
using Volo.Abp.Identity.EntityFrameworkCore;
using Volo.Abp.PermissionManagement.EntityFrameworkCore;
using Volo.Abp.SettingManagement.EntityFrameworkCore;
using Volo.Abp.OpenIddict.EntityFrameworkCore;
using Volo.Abp.TenantManagement;
using Volo.Abp.TenantManagement.EntityFrameworkCore;
using Alberta.ServiceDesk;
using Alberta.ServiceDesk.Facilities;
using Alberta.ServiceDesk.Bookings;
using Alberta.ServiceDesk.Policies;

namespace Alberta.ServiceDesk.EntityFrameworkCore;

[ReplaceDbContext(typeof(IIdentityDbContext))]
[ReplaceDbContext(typeof(ITenantManagementDbContext))]
[ConnectionStringName("Default")]
public class ServiceDeskDbContext :
    AbpDbContext<ServiceDeskDbContext>,
    ITenantManagementDbContext,
    IIdentityDbContext
{
    /* Add DbSet properties for your Aggregate Roots / Entities here. */
    
    public DbSet<Facility> Facilities { get; set; }
    public DbSet<Booking> Bookings { get; set; }
    public DbSet<BookingApproval> BookingApprovals { get; set; }
    public DbSet<PolicyRule> PolicyRules { get; set; }

    #region Entities from the modules

    /* Notice: We only implemented IIdentityProDbContext and ISaasDbContext
     * and replaced them for this DbContext. This allows you to perform JOIN
     * queries for the entities of these modules over the repositories easily. You
     * typically don't need that for other modules. But, if you need, you can
     * implement the DbContext interface of the needed module and use ReplaceDbContext
     * attribute just like IIdentityProDbContext and ISaasDbContext.
     *
     * More info: Replacing a DbContext of a module ensures that the related module
     * uses this DbContext on runtime. Otherwise, it will use its own DbContext class.
     */

    // Identity
    public DbSet<IdentityUser> Users { get; set; }
    public DbSet<IdentityRole> Roles { get; set; }
    public DbSet<IdentityClaimType> ClaimTypes { get; set; }
    public DbSet<OrganizationUnit> OrganizationUnits { get; set; }
    public DbSet<IdentitySecurityLog> SecurityLogs { get; set; }
    public DbSet<IdentityLinkUser> LinkUsers { get; set; }
    public DbSet<IdentityUserDelegation> UserDelegations { get; set; }
    public DbSet<IdentitySession> Sessions { get; set; }

    // Tenant Management
    public DbSet<Tenant> Tenants { get; set; }
    public DbSet<TenantConnectionString> TenantConnectionStrings { get; set; }

    #endregion

    public ServiceDeskDbContext(DbContextOptions<ServiceDeskDbContext> options)
        : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        /* Include modules to your migration db context */

        builder.ConfigurePermissionManagement();
        builder.ConfigureSettingManagement();
        builder.ConfigureBackgroundJobs();
        builder.ConfigureAuditLogging();
        builder.ConfigureFeatureManagement();
        builder.ConfigureIdentity();
        builder.ConfigureOpenIddict();
        builder.ConfigureTenantManagement();
        builder.ConfigureBlobStoring();
        
        /* Configure your own tables/entities inside here */

        builder.Entity<Facility>(b =>
        {
            b.ToTable(ServiceDeskConsts.DbTablePrefix + "Facilities", ServiceDeskConsts.DbSchema);
            b.ConfigureByConvention();
        });

        builder.Entity<Booking>(b =>
        {
            b.ToTable(ServiceDeskConsts.DbTablePrefix + "Bookings", ServiceDeskConsts.DbSchema);
            b.ConfigureByConvention();
            
            b.Property(x => x.BookingNo)
                .IsRequired()
                .HasMaxLength(32);
            
            b.Property(x => x.Purpose)
                .IsRequired()
                .HasMaxLength(256);
            
            // Unique index on BookingNo
            b.HasIndex(x => x.BookingNo)
                .IsUnique();
            
            // Composite index on FacilityId and StartTime for query performance
            b.HasIndex(x => new { x.FacilityId, x.StartTime });
            
            b.HasOne(x => x.Facility)
                .WithMany()
                .HasForeignKey(x => x.FacilityId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        builder.Entity<BookingApproval>(b =>
        {
            b.ToTable(ServiceDeskConsts.DbTablePrefix + "BookingApprovals", ServiceDeskConsts.DbSchema);
            b.ConfigureByConvention();
        });

        builder.Entity<PolicyRule>(b =>
        {
            b.ToTable(ServiceDeskConsts.DbTablePrefix + "PolicyRules", ServiceDeskConsts.DbSchema);
            b.ConfigureByConvention();
        });
    }
}
