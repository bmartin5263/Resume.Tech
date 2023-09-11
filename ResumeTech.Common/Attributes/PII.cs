namespace ResumeTech.Common.Attributes; 

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Field | AttributeTargets.Property)]
public class PIIAttribute : Attribute {
    public bool Sensitive { get; }
    
    public PIIAttribute(bool Sensitive) {
        this.Sensitive = Sensitive;
    }
}