namespace ResumeTech.Common.Attributes; 

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Field | AttributeTargets.Property)]
public class PiiAttribute : Attribute {
    public bool Sensitive { get; }
    
    public PiiAttribute(bool Sensitive = false) {
        this.Sensitive = Sensitive;
    }
}