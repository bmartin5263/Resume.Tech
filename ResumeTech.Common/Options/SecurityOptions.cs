namespace ResumeTech.Common.Options; 

public class SecurityOptions {
    public const string Section = "Security";

    public JwtOptions JWT { get; set; } = new();
}