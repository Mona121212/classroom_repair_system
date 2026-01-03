using System;
using System.Threading.Tasks;
using Alberta.ServiceDesk.Facilities;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Guids;

namespace Alberta.ServiceDesk.Data;

public class FacilityDataSeedContributor : IDataSeedContributor, ITransientDependency
{
    private readonly IRepository<Facility, Guid> _facilityRepository;
    private readonly IGuidGenerator _guidGenerator;

    public FacilityDataSeedContributor(
        IRepository<Facility, Guid> facilityRepository,
        IGuidGenerator guidGenerator)
    {
        _facilityRepository = facilityRepository;
        _guidGenerator = guidGenerator;
    }

    public async Task SeedAsync(DataSeedContext context)
    {
        if (await _facilityRepository.GetCountAsync() > 0)
        {
            return; // Already seeded
        }

        var facilities = new[]
        {
            new Facility(_guidGenerator.Create())
            {
                Name = "Main Auditorium",
                Description = "Large auditorium for events and presentations",
                OwnerUnit = "Administration",
                Type = "Auditorium",
                RequiresApproval = true,
                Capacity = 500
            },
            new Facility(_guidGenerator.Create())
            {
                Name = "Computer Lab A",
                Description = "Computer lab with 30 workstations",
                OwnerUnit = "Computer Science",
                Type = "Lab",
                RequiresApproval = false,
                Capacity = 30
            },
            new Facility(_guidGenerator.Create())
            {
                Name = "Sports Hall",
                Description = "Multi-purpose sports hall",
                OwnerUnit = "Physical Education",
                Type = "Sports",
                RequiresApproval = true,
                Capacity = 100
            },
            new Facility(_guidGenerator.Create())
            {
                Name = "Library Study Room 1",
                Description = "Small study room for group work",
                OwnerUnit = "Library",
                Type = "Study Room",
                RequiresApproval = false,
                Capacity = 8
            }
        };

        foreach (var facility in facilities)
        {
            await _facilityRepository.InsertAsync(facility, autoSave: true);
        }
    }
}
