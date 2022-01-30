namespace IdentityServer.Security.Authorization
{
    public class HRManagerProbationRequirement : IAuthorizationRequirement
    {
        public int ProbationMonths { get; }

        public HRManagerProbationRequirement(int probationMonths)
        {
            this.ProbationMonths = probationMonths;
        }
    }
}
