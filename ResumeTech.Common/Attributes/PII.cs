namespace ResumeTech.Common.Attributes; 

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Field | AttributeTargets.Property)]
public class PII : Attribute {
    public bool Sensitive { get; }
    
    public PII(bool Sensitive) {
        this.Sensitive = Sensitive;
    }
}