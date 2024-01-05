using Moq;
using ResumeTech.Common.Actions;
using ResumeTech.Common.Auth;
using ResumeTech.Common.Error;
using ResumeTech.Common.Events;
using ResumeTech.Common.Utility;
using ResumeTech.TestUtil;

namespace ResumeTech.Common.UnitTest; 

public class ExecTest {
    private Mock<IEventDispatcher> eventDispatcher = null!;
    private UnitOfWorkMock unitOfWork = null!;
    private Exec subject = null!;

    [SetUp]
    public void SetUp() {
        eventDispatcher = new Mock<IEventDispatcher>();
        unitOfWork = new UnitOfWorkMock();
        subject = new Exec(unitOfWork, eventDispatcher.Object);
    }

    [Test]
    public async Task ExecuteCommand_WithUserHavingValidAuthorization_ShouldSucceed() {
        var user = CreateUser(new[] { RoleName.User });
        unitOfWork.Login(user);
        
        var result = await subject.Command(new BasicCommand(), "123");
        
        Assert.That(result, Is.EqualTo(123));
    }

    [Test]
    public void ExecuteCommand_WithUserNotHavingValidRole_ShouldThrow() {
        var user = CreateUser(Enumerable.Empty<RoleName>());
        unitOfWork.Login(user);

        Assert.ThrowsAsync<AuthorizationException>(() => subject.Command(new BasicCommand(), "123"));
    }

    [Test]
    public async Task ExecutePublicCommand_WithNotLoggedInUser_ShouldSucceed() {
        var user = UserDetails.NotLoggedIn;
        unitOfWork.Login(user);
        
        var result = await subject.Command(new PublicCommand(), "123");
        
        Assert.That(result, Is.EqualTo(123));
    }
    
    [Test]
    public async Task ExecuteAdminOnlyCommand_WithAdminUser_ShouldSucceed() {
        var user = CreateUser(new[] { RoleName.Admin });
        unitOfWork.Login(user);
        
        var result = await subject.Command(new AdminOnlyCommand(), "123");
        
        Assert.That(result, Is.EqualTo(123));
    }
    
    [Test]
    public void ExecuteAdminOnlyCommand_WithoutAdminUser_ShouldThrow() {
        var user = CreateUser(new[] { RoleName.User });
        unitOfWork.Login(user);
        
        Assert.ThrowsAsync<AuthorizationException>(() => subject.Command(new AdminOnlyCommand(), "123"));
    }

    private static UserDetails CreateUser(IEnumerable<RoleName> roles) {
        return new UserDetails(Id: UserId.Generate(), Username: "test", Roles: roles);
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

class AdminOnlyCommand : Command<string, int> {
    public override string Name => "AdminOnly";
    public override Roles UserRoles => Roles.AdminOnly();

    public override Task<int> Run(string args) {
        return Task.FromResult(int.Parse(args));
    }
}