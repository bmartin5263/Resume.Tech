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

public class JobTest2 {

    private Authorizer<Job> Authorizer = null!;
    private IJobRepository JobRepository = null!;
    private JobManager JobManager = null!;

    private CreateJob CreateJob = null!;

    [SetUp]
    public void SetUp() {
        Authorizer = new Authorizer<Job>(new List<IAccessFilter<Job>>(), TestAuth.RandomUserProvider());
        JobRepository = new JobInMemoryRepository();
        JobManager = new JobManager(JobRepository, Authorizer);

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
        
        Assert.True(result.RecursiveEquals(new JobDto(
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
        ), "Id", "CreatedAt", "UpdatedAt", "DeletedAt"));
        Assert.That(result.CompanyName, Is.EqualTo("Jewel"));
    }
}