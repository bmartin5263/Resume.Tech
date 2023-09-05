namespace ResumeTech.Domain.Common; 

public class ContactDetails {
    public PersonName? Name { get; set; }
    public PhoneNumber? PhoneNumber { get; set; }
    public Uri? Website { get; set; }
    public Address? Address { get; set; }

    public ContactDetails() {
        
    }
    
    public ContactDetails(PersonName name) {
        Name = name;
    }
}