using Moq;
using ResumeTech.Common.Actions;
using ResumeTech.Common.Auth;
using ResumeTech.Common.Events;
using ResumeTech.Common.Utility;

namespace ResumeTech.Common.UnitTest; 

public class ExecTest {
    private IUnitOfWork unitOfWork = null!;
    private IEventDispatcher eventDispatcher = null!;
    private Exec subject = null!;

    [SetUp]
    public void SetUp() {
        var unitOfWorkMock = new Mock<IUnitOfWork>();
        unitOfWorkMock.Setup(m => m.Commit())
            .Returns(Task.FromResult<ICollection<IDomainEvent>>(new List<IDomainEvent>()));

        var eventDispatcherMock = new Mock<IEventDispatcher>();

        unitOfWork = unitOfWorkMock.Object;
        eventDispatcher = eventDispatcherMock.Object;
        subject = new Exec(unitOfWork, eventDispatcher);
    }

    [Test]
    public async Task ExecuteCommand_WithUserHavingValidAuthorization_ShouldReturnResult() {
        var user = new UserDetails(Id: UserId.Generate(), Username: "test", Roles: new[] { RoleName.User });
        var result = await subject.Command(new BasicCommand(), user, "123");
        
        Assert.That(result, Is.EqualTo(123));
    }

    [Test]
    public void ExecuteCommand_WithUserNotHavingValidRole_ShouldThrow() {
        var user = new UserDetails(Id: UserId.Generate(), Username: "test", Roles: Enumerable.Empty<RoleName>());

        Assert.ThrowsAsync<AccessDeniedException>(() => subject.Command(new BasicCommand(), user, "123"));
    }
}

class BasicCommand : Command<string, int> {
    public override string Name => "Basic";

    public override Task<int> Run(string args) {
        return Task.FromResult(int.Parse(args));
    }
}

class PublicCommand : Command<string, int> {
    public override string Name => "Public";
    public override Roles UserRoles => Roles.Public();

    public override Task<int> Run(string args) {
        return Task.FromResult(int.Parse(args));
    }
}