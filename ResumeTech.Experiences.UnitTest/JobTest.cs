using ResumeTech.Common.Auth;
using ResumeTech.Common.Utility;
using ResumeTech.Experiences.Common;
using ResumeTech.Experiences.Contacts;
using ResumeTech.Experiences.Jobs;

namespace ResumeTech.Experiences.UnitTest;

public class JobTest {

    [Test]
    public void CreateJob_ShouldHaveSetValues()
    {
        var owner = UserId.Generate();
        var location = new Location(City: "Chicago", State: "Illinois", Country: "USA");
        var job = new Job(OwnerId: owner, CompanyName: "Jewel Osco", Location: location, Positions: new [] {
            new Position(
                Title: "Bagger", 
                Dates: new DateOnlyRange("09/12/2015".SlashedDate()),
                BulletPoints: new [] {
                    new BulletPoint("Bagged groceries"),
                    new BulletPoint("Cleaned the floor")
                }
            )
        });
        Assert.Multiple(() =>
        {
            Assert.That(job.Id, Is.Not.EqualTo(JobId.Empty));
            Assert.That(job.OwnerId, Is.EqualTo(owner));
            Assert.That(job.CreatedAt, Is.LessThanOrEqualTo(DateTimeOffset.UtcNow));
            Assert.That(job.UpdatedAt, Is.Null);
            Assert.That(job.DeletedAt, Is.Null);
            Assert.That(job.CompanyName, Is.EqualTo("Jewel Osco"));
            Assert.That(job.Location, Is.EqualTo(location));
            Assert.That(job.Positions, Has.Count.EqualTo(1));
        });
        
        var position = job.Positions[0];
        Assert.Multiple(() =>
        {
            Assert.That(position.Id, Is.Not.EqualTo(PositionId.Empty));
            Assert.That(position.CreatedAt, Is.LessThanOrEqualTo(DateTimeOffset.UtcNow));
            Assert.That(position.UpdatedAt, Is.Null);
            Assert.That(position.Title, Is.EqualTo("Bagger"));
        });
    }
}