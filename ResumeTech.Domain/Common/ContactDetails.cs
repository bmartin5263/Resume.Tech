namespace ResumeTech.Domain.Common; 

public sealed record ContactDetails(
    PersonName? Name = null,
    PhoneNumber? PhoneNumber = null,
    Uri? Website = null,
    Address? Address = null
);