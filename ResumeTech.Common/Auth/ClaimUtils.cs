using System.Security.Claims;

namespace ResumeTech.Common.Auth; 

public static class ClaimUtils {

    public static bool TypeContains(this Claim claim, string str) {
        return claim.Type.Contains(str, StringComparison.InvariantCultureIgnoreCase);
    }
    
    public static bool TypeContains(this Claim claim, params string[] str) {
        return Array.Exists(str, s => claim.Type.Contains(s, StringComparison.InvariantCultureIgnoreCase));
    }
    
}