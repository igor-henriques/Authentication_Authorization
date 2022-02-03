namespace Auth.Controllers;

public class Account : Controller
{
    private readonly UserManager<IdentityUser> userManager;
    private readonly SignInManager<IdentityUser> signManager;
    private readonly IAuthService authService;

    public Account(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signManager, IAuthService authService)
    {
        this.userManager = userManager;
        this.signManager = signManager;
        this.authService = authService;
    }

    public IActionResult Register()
    {
        return View();
    }

    [HttpGet]
    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync();

        return RedirectToAction("Index", "Home");
    }

    [HttpPost]
    public async Task<IActionResult> Register([FromForm] RegisterVM register)
    {
        var identity = new IdentityUser
        {
            Email = register.Email,
            UserName = register.Email
        };

        var result = await userManager.CreateAsync(identity, register.Password);

        if (!result.Succeeded)
        {
            result.Errors
                .ToList()
                .ForEach(error => ModelState.AddModelError(string.Empty, error.Description));

            return View();
        }

        var confirmationToken = await userManager.GenerateEmailConfirmationTokenAsync(identity);

        return RedirectToAction(nameof(Login));
    }    

    [HttpPost("Login")]
    public async ValueTask<IActionResult> Login([FromForm] Credential credential, CancellationToken cancellationToken = default)
    {
        if (!ModelState.IsValid) return Unauthorized("Informações Inválidas");

        var signInResult = await signManager.PasswordSignInAsync(credential.Username, credential.Password, credential.RememberMe, false);

        if (!signInResult.Succeeded) return Unauthorized("Login ou senha incorretos");

        var bearerToken = JsonConvert.DeserializeObject<Token>(HttpContext.Session.GetString("access_token") ?? string.Empty);

        if (bearerToken is null or default(Token) | DateTime.Now >= bearerToken?.ExpiresAt)
        {
            bearerToken = await authService.Authenticate(credential, cancellationToken);

            HttpContext.Session.SetString("access_token", JsonConvert.SerializeObject(bearerToken));
        }

        return RedirectToAction("Index", "Home");
    }
}