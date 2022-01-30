namespace IdentityServer.Controllers;

[Authorize(Policy = "MustBeFromHumanResourceDepartment")]
public class HumanResourceController : Controller
{
    public IActionResult Index()
    {
        var token = HttpContext.Session.GetString("access_token");
        return View();
    }

    [Authorize(Policy = "HumanResourceStaff")]
    public IActionResult Staff()
    {
        return View();
    }
}