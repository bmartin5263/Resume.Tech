namespace ResumeTech.Experiences.Contacts.Dto; 

public sealed record CreateContactInfoRequest(
    PersonName Name,
    Location Location, 
    IList<Link>? Links
);
