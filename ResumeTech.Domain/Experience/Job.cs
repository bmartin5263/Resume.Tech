using ResumeTech.Domain.Common;
using ResumeTech.Domain.Util;

namespace ResumeTech.Domain.Experience; 

public class Job {
    public string CompanyName { get; set; }
    public string? Location { get; set; }
    public PhoneNumber? PhoneNumber { get; set; }
    
    private IList<JobPosition>? _positions;
    public IReadOnlyList<JobPosition> Positions => _positions.ToReadOnly();

    // Common Entity Properties
    public JobId Id { get; private set; } = JobId.Generate();
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset? UpdatedAt { get; set; }
    public DateTimeOffset? DeletedAt { get; set; }
    
    private Job() {
        CompanyName = null!;
    }
    
    public Job(string companyName) {
        CompanyName = companyName;
    }

    public void AddPosition(JobPosition position) {
        _positions ??= new List<JobPosition>();
        _positions.Add(position);
    }
    
    
}