using System.Collections;
using System.Collections.Immutable;
using System.Collections.ObjectModel;

namespace ResumeTech.Common.Utility;

public static class ReadOnly {
    public static IReadOnlyList<T> List<T>() {
        return new ReadOnlyCollection<T>(ImmutableList<T>.Empty);
    }

    public static IReadOnlySet<T> Set<T>() {
        return new ReadOnlySet<T>(new HashSet<T>()); // avoid allocation?
    }
    
    public static IReadOnlySet<T> SetOf<T>(params T[] items) {
        return new ReadOnlySet<T>(items.ToImmutableHashSet()); // avoid allocation?
    }

    public static IReadOnlySet<T> AsReadOnly<T>(this ISet<T> self) {
        return new ReadOnlySet<T>(self); // avoid allocation?
    }

    public static IReadOnlyList<T> ToReadOnly<T>(this IList<T>? self) {
        return self?.AsReadOnly() ?? List<T>();
    }

    public static IReadOnlySet<T> ToReadOnly<T>(this ISet<T>? self) {
        return self?.AsReadOnly() ?? Set<T>();
    }
}

public class ReadOnlySet<T> : IReadOnlySet<T> {
    private ISet<T> Impl { get; }

    public ReadOnlySet(ISet<T> impl) {
        Impl = impl;
    }

    public IEnumerator<T> GetEnumerator() {
        return Impl.GetEnumerator();
    }

    public bool Contains(T item) {
        return Impl.Contains(item);
    }

    public int Count => Impl.Count;

    public bool IsReadOnly => true;

    public bool IsProperSubsetOf(IEnumerable<T> other) {
        return Impl.IsProperSubsetOf(other);
    }

    public bool IsProperSupersetOf(IEnumerable<T> other) {
        return Impl.IsProperSupersetOf(other);
    }

    public bool IsSubsetOf(IEnumerable<T> other) {
        return Impl.IsSubsetOf(other);
    }

    public bool IsSupersetOf(IEnumerable<T> other) {
        return Impl.IsSupersetOf(other);
    }

    public bool Overlaps(IEnumerable<T> other) {
        return Impl.Overlaps(other);
    }

    public bool SetEquals(IEnumerable<T> other) {
        return Impl.SetEquals(other);
    }

    IEnumerator IEnumerable.GetEnumerator() {
        return GetEnumerator();
    }
}