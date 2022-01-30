namespace IdentityServer.Controllers;

public class AccountController : Controller
{
    private readonly IAuthService authService;

    public AccountController(IAuthService authService)
    {
        this.authService = authService;
    }

    [HttpGet("Login")]
    public IActionResult Login()
    {
        return View(new Credential { Username = "admin" });
    }

    [HttpPost("Logout")]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync();

        return RedirectToAction("Index", "Home");
    }

    [HttpPost("Login")]
    public async ValueTask<IActionResult> Login([FromForm] Credential credential, CancellationToken cancellationToken = default)
    {
        if (!ModelState.IsValid) return Unauthorized("Informações Inválidas");

        if (!credential.Equals(Credential.MokedCredential with { RememberMe = credential.RememberMe })) return Unauthorized("Login ou senha incorretos");

        var bearerToken = JsonConvert.DeserializeObject<Token>(HttpContext.Session.GetString("access_token") ?? string.Empty);

        if (bearerToken is null | DateTime.Now >= bearerToken?.ExpiresAt)
        {
            bearerToken = await authService.Authenticate(credential, cancellationToken);

            HttpContext.Session.SetString("access_token", JsonConvert.SerializeObject(bearerToken));
        }

        return Ok(bearerToken);
    }

    [HttpGet]
    public IActionResult AccessDenied()
    {
        return View();
    }
}