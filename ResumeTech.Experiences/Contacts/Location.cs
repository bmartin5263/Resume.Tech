using ResumeTech.Common.Utility;

namespace ResumeTech.Experiences.Contacts;

public sealed record Location {
    public const int MaxFieldLength = 80;
    
    public string? City { get; }
    public string? State { get; }
    public string? Country { get; }

    public Location(string? City = null, string? State = null, string? Country = null) {
        this.City = City?.AssertMaxTrimmedLength(MaxFieldLength, "City");
        this.State = State?.AssertMaxTrimmedLength(MaxFieldLength, "State");
        this.Country = Country?.AssertMaxTrimmedLength(MaxFieldLength, "Country");
    }

    public void Deconstruct(
        out string? City,
        out string? State,
        out string? Country
    ) {
        City = this.City;
        State = this.State;
        Country = this.Country;
    }
}