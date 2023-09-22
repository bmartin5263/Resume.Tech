using ResumeTech.Common.Exceptions;

namespace ResumeTech.Common.Domain; 

public record EmailAddress : IWrapper<string> {
    public string Value { get; init; }
    
    public EmailAddress(string Value) {
        this.Value = Value;
        if (!Value.Contains('@')) {
            throw new AppError(UserMessage: $"Malformed email address: {Value}").ToException();
        }
    }
    
    public void Deconstruct(out string Value) {
        Value = this.Value;
    }
}