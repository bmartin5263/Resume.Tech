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

    [Test]
    public void EqualsTest() {
        var position1 = new Position(Title: "Software Bagger", Dates: new DateOnlyRange(Clock.Now, Clock.Now)) {
            BulletPoints = {
                new BulletPoint("Some bullet point")
            }
        };
        var position2 = new Position(Title: "Software Bagger", Dates: new DateOnlyRange(Clock.Now, Clock.Now)) {
            BulletPoints = {
                new BulletPoint("Some bullet point")
            }
        };
        
        var job1 = new Job(OwnerId: UserId.Generate(), CompanyName: "Jewel", Positions: new[] { position1 });
        var job2 = new Job(OwnerId: UserId.Generate(), CompanyName: "Jewel", Positions: new[] { position2 });
        var someObject = new object();
        
        Assert.That(job1 == job2, Is.False);
        Assert.That(job2 == job1, Is.False);
        Assert.That(job1 != job2, Is.True);
        Assert.That(job2 != job1, Is.True);
        Assert.That(job1.Equals(job1), Is.True);
        Assert.That(job2.Equals(job2), Is.True);
        Assert.That(job1.Equals(job2), Is.False);
        Assert.That(job2.Equals(job1), Is.False);
        Assert.That(job1.Equals(someObject), Is.False);
        Assert.That(job2.Equals(someObject), Is.False);
        Assert.That(job1.Equals(null), Is.False);
        Assert.That(job2.Equals(null), Is.False);
        Assert.That(job1 == null, Is.False);
        Assert.That(job2 == null, Is.False);
        Assert.That(job1 != null, Is.True);
        Assert.That(job2 != null, Is.True);
        Assert.That(null == job1, Is.False);
        Assert.That(null == job2, Is.False);
        Assert.That(null != job1, Is.True);
        Assert.That(null != job2, Is.True);
    }
}