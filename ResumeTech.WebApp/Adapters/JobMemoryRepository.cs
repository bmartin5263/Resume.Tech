using ResumeTech.Common.Utility;
using ResumeTech.Experiences.Jobs;

namespace ResumeTech.Application.Adapters; 

public class JobMemoryRepository : IJobRepository {
    protected IDictionary<JobId, Job> Jobs { get; } = new Dictionary<JobId, Job>();

    public Job? FindById(JobId id) { 
        return Jobs.Get(id, orElse: null);
    }

    public void Add(Job entity) {
        Jobs[entity.Id] = entity;
    }
}