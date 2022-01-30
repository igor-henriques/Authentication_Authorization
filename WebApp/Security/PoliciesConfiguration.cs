namespace IdentityServer.Security;

public static class PoliciesConfiguration
{
    public static void ConfigurePolicies(this AuthorizationOptions option)
    {
        option.AddPolicy("Admin",
            policy => policy.RequireClaim("Admin"));

        option.AddPolicy("HumanResourceStaff",
            policy => policy.RequireClaim("Department", "HR")
            .RequireClaim("Manager")
            .Requirements.Add(new HRManagerProbationRequirement(3)));

        option.AddPolicy("MustBeFromHumanResourceDepartment",
             policy => policy.RequireClaim("Department", "HR"));
    }
}