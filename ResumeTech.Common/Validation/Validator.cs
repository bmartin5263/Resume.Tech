using ResumeTech.Common.Error;
using ResumeTech.Common.Utility;

namespace ResumeTech.Common.Validation; 

public class Validator<T> {
    public T Target { get; }
    public string Path { get; }
    public List<AppSubError> SubErrors { get; } = new();

    public Validator(T target, string path) {
        Target = target;
        Path = path;
    }

    public static Validator<T> Create(T obj, string path = "") {
        return new Validator<T>(obj, path);
    }

    public Validator<T> Check(Action<T> validation) {
        try {
            validation(Target);
        }
        catch (AppException e) {
            SubErrors.AddRange(e.Error.SubErrors);
            if (e.Error.UserMessage != null) {
                SubErrors.Add(new AppSubError(Path: Path, Message: e.Error.UserMessage));
            }
        }
        return this;
    }

    public void ThrowIfFailed() {
        if (SubErrors.IsNotEmpty()) {
            throw new AppError(
                SubErrors: SubErrors
            ).ToException();
        }
    }
}

public static class ValidatorUtils {
    
    public static Validator<T> CheckCollection<T, C>(this Validator<T> self, string fieldName, Func<T, ICollection<C>> getter, Action<Validator<ICollection<C>>> validations) {
        var validator = Validator<ICollection<C>>.Create(getter(self.Target), fieldName);
        validations(validator);
        self.SubErrors.AddRange(validator.SubErrors);
        return self;
    }

    public static Validator<ICollection<T>> CheckEach<T>(this Validator<ICollection<T>> self, Action<Validator<T>> validations) {
        int counter = 0;
        foreach (var obj in self.Target) {
            var validator = Validator<T>.Create(obj, $"[{counter++}]");
            validations(validator);
            self.SubErrors.AddRange(validator.SubErrors);
        }
        return self;
    }

}