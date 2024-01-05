using System.Collections;
using System.Collections.Immutable;
using ResumeTech.Common.Auth;
using ResumeTech.Common.Error;
using ResumeTech.Common.Utility;

namespace ResumeTech.Common.Actions;

public interface RoleCheckMode2 {
    public static AnyRole Any { get; } = new();
    public static AllRoles All { get; } = new();
    
    void Authorize(UserDetails user, IReadOnlySet<RoleName> allowedRoles);
}

public class AnyRole : RoleCheckMode2 {
    public void Authorize(UserDetails user, IReadOnlySet<RoleName> allowedRoles) {
        if (!user.Roles.ContainsAny(allowedRoles)) {
            throw new AuthorizationException(
                $"{user.Username} roles {user.Roles.ToExpandedString()} did not contain any of {allowedRoles.ToExpandedString()}"
            );
        }
    }
}

public class AllRoles : RoleCheckMode2 {
    public void Authorize(UserDetails user, IReadOnlySet<RoleName> allowedRoles) {
        if (!user.Roles.ContainsAll(allowedRoles)) {
            throw new AuthorizationException(
                $"{user.Username} roles {user.Roles.ToExpandedString()} did not contain all of {allowedRoles.ToExpandedString()}"
            );
        }
    }
}

public sealed class RoleCheckMode3 {
    public static RoleCheckMode3 Any { get; } = new((user, allowedRoles) => {
        if (!user.Roles.ContainsAny(allowedRoles)) {
            throw new AuthorizationException(
                $"{user.Username} roles {user.Roles.ToExpandedString()} did not contain any of {allowedRoles.ToExpandedString()}"
            );
        }
    });
    
    public static RoleCheckMode3 All { get; } = new((user, allowedRoles) => {
        if (!user.Roles.ContainsAll(allowedRoles)) {
            throw new AuthorizationException(
                $"{user.Username} roles {user.Roles.ToExpandedString()} did not contain all of {allowedRoles.ToExpandedString()}"
            );
        }
    });
    
    private System.Action<UserDetails, IReadOnlySet<RoleName>> Authorizer { get; }

    public RoleCheckMode3(System.Action<UserDetails, IReadOnlySet<RoleName>> authorizer) {
        Authorizer = authorizer;
    }

    public void Authorize(UserDetails user, IReadOnlySet<RoleName> allowedRoles) {
        Authorizer(user, allowedRoles);
    }
}

public class Roles : IEnumerable<RoleName> {
    private IReadOnlySet<RoleName> Values { get; }
    private RoleCheckMode3 Mode { get; }
    
    public Roles(IReadOnlySet<RoleName> roles, RoleCheckMode3 mode) {
        Values = roles;
        Mode = mode;

        if (roles.Contains(RoleName.Admin) && (roles.Count > 1 || mode != RoleCheckMode3.All)) {
            throw new ConfigurationException("Roles must contain only admin and mode must be All when doing Admin only");
        }
    }

    public static Roles Any(params RoleName[] roles) {
        return new Roles(ReadOnly.SetOf(roles), RoleCheckMode3.Any);
    }

    public static Roles All(params RoleName[] roles) {
        return new Roles(ReadOnly.SetOf(roles), RoleCheckMode3.All);
    }

    public static Roles AdminOnly() {
        return new Roles(ReadOnly.SetOf(RoleName.Admin), RoleCheckMode3.All);
    }

    public static Roles Public() {
        return new Roles(ImmutableHashSet<RoleName>.Empty, RoleCheckMode3.Any);
    }

    public void Authorize(UserDetails user) {
        Mode.Authorize(user, Values);
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