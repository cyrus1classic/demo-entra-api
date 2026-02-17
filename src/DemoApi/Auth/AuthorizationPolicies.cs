using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace DemoApi.Auth;

public static class AuthorizationPolicies
{
    public static string ScopePolicy(string scope) => $"Scopes:{scope}";
    public static string RolePolicy(string role) => $"Roles:{role}";

    public static AuthorizationPolicyBuilder RequireScopeOrRole(this AuthorizationPolicyBuilder builder, string scope, string role)
    {
        builder.RequireAssertion(context =>
        {
            var scopeClaim = context.User.FindFirst("scp")?.Value;
            var hasScope = !string.IsNullOrWhiteSpace(scopeClaim)
                && scopeClaim.Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
                    .Contains(scope, StringComparer.OrdinalIgnoreCase);

            var hasRole = context.User.FindAll(ClaimTypes.Role)
                    .Select(c => c.Value)
                    .Concat(context.User.FindAll("roles").Select(c => c.Value))
                    .Any(r => string.Equals(r, role, StringComparison.OrdinalIgnoreCase));

            return hasScope || hasRole;
        });

        return builder;
    }
}
