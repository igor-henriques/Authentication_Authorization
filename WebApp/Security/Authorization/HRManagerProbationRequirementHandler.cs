namespace WebApp.Security.Authorization;

public class HRManagerProbationRequirementHandler : AuthorizationHandler<HRManagerProbationRequirement>
{
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, HRManagerProbationRequirement requirement)
    {
        if (!context.User.HasClaim(x => x.Type == "EmploymentDate")) return Task.CompletedTask;

        var dateHired = DateTime.Parse(context.User.FindFirst(x => x.Type == "EmploymentDate").Value);
        var timeHired = DateTime.Now - dateHired;

        if (timeHired.Days > requirement.ProbationMonths * 30)
        {
            context.Succeed(requirement);
        }

        return Task.CompletedTask;
    }
}