using ResumeTech.Common.Error;

namespace ResumeTech.Common.Domain; 

public record EmailAddress : IWrapper<string> {
    public string Value { get; init; }
    
    public EmailAddress(string Value) {
        this.Value = Value;
        if (!Value.Contains('@')) {
            throw new ArgumentException($"Malformed email address: {Value}");
        }
    }
    
    public void Deconstruct(out string Value) {
        Value = this.Value;
    }
}