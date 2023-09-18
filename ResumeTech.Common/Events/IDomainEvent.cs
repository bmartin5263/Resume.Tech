using ResumeTech.Common.Utility;

namespace ResumeTech.Common.Events; 

public interface IDomainEvent {
    public string Name { get; }
    public LogPolicy LogPolicy => LogPolicy.IncludeArguments;
}