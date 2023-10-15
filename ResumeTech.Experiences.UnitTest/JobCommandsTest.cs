using ResumeTech.Common.Auth;
using ResumeTech.Experiences.Common;
using ResumeTech.Experiences.Contacts;
using ResumeTech.Experiences.Jobs;
using ResumeTech.Experiences.Jobs.Actions;
using ResumeTech.Experiences.Jobs.Dto;
using ResumeTech.Identities.Auth;
using ResumeTech.Identities.Auth.Filters;
using ResumeTech.Persistence.InMemory.Repositories;
using ResumeTech.TestUtil;

namespace ResumeTech.Experiences.UnitTest; 

public class JobCommandsTest {

    private IJobRepository JobRepository = null!;
    private JobManager JobManager = null!;
    private CreateJob CreateJob = null!;

    [SetUp]
    public void SetUp() {
        JobRepository = new JobInMemoryRepository();
        JobManager = new JobManager(JobRepository, new UserDetailsProvider(UserDetails.SystemUser));
        CreateJob = new CreateJob(JobManager);
    }

    [Test]
    public async Task CreateJob_ShouldHaveCorrectValues() {
        JobDto result = await CreateJob.Run(new CreateJobRequest(
            CompanyName: "Jewel",
            Location: new Location(),
            Positions: new List<AddPositionRequest> {
                new(
                    Title: "Bagger",
                    Dates: new DateOnlyRange(DateOnly.FromDateTime(DateTime.Now)),
                    BulletPoints: new List<string> {
                        "Bagged grocercies",
                        "Cleaned the registers"
                    }
                )
            }
        ));
        
        Assert.That(result.RecursiveEquals(new JobDto(
            OwnerId: UserDetails.SystemUser.Id,
            CompanyName: "Jewel",
            Location: new Location(),
            Positions: new List<PositionDto> {
                new(
                    Title: "Bagger",
                    Dates: new DateOnlyRange(DateOnly.FromDateTime(DateTime.Now)),
                    BulletPoints: new List<BulletPoint> {
                        new("Bagged grocercies"),
                        new("Cleaned the registers")
                    }
                )
            }
        ), "Id", "CreatedAt", "UpdatedAt", "DeletedAt"), Is.True);
    }
}