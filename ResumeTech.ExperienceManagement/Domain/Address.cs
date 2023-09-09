namespace ResumeTech.Domain.Common;

public sealed record Address(string City, string State, string Country) {
    private Address() : this("", "", "") {}
}