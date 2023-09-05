namespace ResumeTech.Domain.Experience; 

public class JobPosition {
    public string Title { get; set; }
    public DateOnly StartDate { get; set; }
    public DateOnly? EndDate { get; set; }
    public IList<BulletPoint> BulletPoints { get; set; } = new List<BulletPoint>();

    public JobPosition(string title, DateOnly startDate) {
        Title = title;
        StartDate = startDate;
    }
}