using ResumeTech.Common.Utility;

namespace ResumeTech.Experiences.Contacts;

public sealed record PersonName {
    public const int MaxFieldLength = 80;
    
    public string FirstName { get; }
    public string? MiddleName { get; }
    public string? LastName { get; }

    private PersonName() {
        FirstName = null!;
        MiddleName = null!;
        LastName = null!;
    }

    public PersonName(string FirstName, string? MiddleName = null, string? LastName = null) {
        this.FirstName = FirstName.AssertMaxTrimmedLength(MaxFieldLength, "First name");
        this.MiddleName = MiddleName?.AssertMaxTrimmedLength(MaxFieldLength, "Middle Name");
        this.LastName = LastName?.AssertMaxTrimmedLength(MaxFieldLength, "Last Name");
    }

    public void Deconstruct(
        out string? FirstName,
        out string? MiddleName,
        out string? LastName
    ) {
        FirstName = this.FirstName;
        MiddleName = this.MiddleName;
        LastName = this.LastName;
    }
}