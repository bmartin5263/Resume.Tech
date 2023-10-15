using ResumeTech.Common.Utility;
using ResumeTech.Experiences.Common;
using ResumeTech.Experiences.Contacts;
using ResumeTech.Experiences.Jobs;
using ResumeTech.Organization.Resumes;

namespace ResumeTech.Organization.UnitTest;

public class ResumeTests {
    [SetUp]
    public void Setup() {
        
    }

    [Test]
    public void Test1() {
        Clock.MockTime = DateTimeOffset.Now;
        var resume = new Resume(Nickname: "My Awesome Resume", ContactInfoId: ContactInfoId.Generate()) {
            Jobs = {
                new ResumeJob(
                    JobId: JobId.Generate(),
                    Positions: new List<ResumePosition> {
                        new(PositionId: PositionId.Generate()) {
                            BulletPoints = {
                                new ResumeBulletPoint(
                                    ReferenceBulletPointId: BulletPointId.Generate()
                                )
                            }
                        }
                    }
                )
            }
        };
        
        Assert.That(resume.Id, Is.Not.EqualTo(ResumeId.Empty));
        Assert.That(resume.CreatedAt, Is.EqualTo(Clock.Now));
    }
}