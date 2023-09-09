namespace ResumeTech.Common.Domain;

/**
 * Interface representing classes who's only purpose is to wrap another non-null object, with the
 * intention of giving it additional type-safety. One example is wrapping a plain `string` in a
 * `RoutingNumber` object to provide additional type information while retaining the `string` as
 * the implementation
 *
 * The purpose of implementing this interface is that some app configuration automatically scans for
 * classes implementing `IWrapper` and can perform some special function with them. Such as when writing
 * json responses we can omit the wrapper object and instead inline `Value`.
 */
public interface IWrapper<out T> where T : notnull {
    T Value { get; }
}