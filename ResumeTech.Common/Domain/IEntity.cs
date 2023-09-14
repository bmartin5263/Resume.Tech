namespace ResumeTech.Common.Domain;

public interface IEntity<out ID> {
    public ID Id { get; }
}