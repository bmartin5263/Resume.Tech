using System.Collections;
using System.Collections.Immutable;
using ResumeTech.Common.Auth;
using ResumeTech.Common.Utility;

namespace ResumeTech.Common.Actions; 

public class Roles : IEnumerable<RoleName> {
    private IReadOnlySet<RoleName> Values { get; }
    private RoleCheckMode Mode { get; }
    
    public Roles(IReadOnlySet<RoleName> roles, RoleCheckMode mode) {
        Values = roles;
        Mode = mode;
    }

    public static Roles Any(params RoleName[] roles) {
        return new Roles(ReadOnly.SetOf(roles), RoleCheckMode.Any);
    }

    public static Roles All(params RoleName[] roles) {
        return new Roles(ReadOnly.SetOf(roles), RoleCheckMode.All);
    }

    public static Roles AdminOnly() {
        return new Roles(ReadOnly.SetOf(RoleName.Admin), RoleCheckMode.All);
    }

    public static Roles Public() {
        return new Roles(ImmutableHashSet<RoleName>.Empty, RoleCheckMode.Any);
    }

    public void Authorize(UserDetails user) {
        switch (Mode) {
            case RoleCheckMode.Any:
                if (!user.Roles.ContainsAny(Values)) {
                    throw new AccessDeniedException(
                        $"{user.Username} roles {user.Roles.ToExpandedString()} did not contain any of {Values.ToExpandedString()}"
                    );
                }
                return;
            case RoleCheckMode.All:
                if (!user.Roles.ContainsAll(Values)) {
                    throw new AccessDeniedException(
                        $"{user.Username} roles {user.Roles.ToExpandedString()} did not contain all of {Values.ToExpandedString()}"
                    );
                }
                return;
            default:
                throw new ArgumentOutOfRangeException($"{Mode} is not handled");
        }
    }

    public bool IsEmpty() {
        return Values.Count == 0;
    }

    public IEnumerator<RoleName> GetEnumerator() {
        return Values.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator() {
        return Values.GetEnumerator();
    }
}