namespace ResumeTech.Common.Options; 

public class UserOptions {
    public string AdminId { get; set; } = string.Empty;
    public string AdminUsername { get; set; } = string.Empty;
    public string AdminEmail { get; set; } = string.Empty;
    public bool RequireConfirmedEmail { get; set; } = false;
}