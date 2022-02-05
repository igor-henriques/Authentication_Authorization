namespace Auth.Controllers;

public class Account : Controller
{
    private readonly UserManager<IdentityUser> userManager;
    private readonly SignInManager<IdentityUser> signManager;
    private readonly IAuthService authService;
    private readonly IEmailService emailService;

    public Account(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signManager, 
        IAuthService authService, IEmailService emailService)
    {
        this.userManager = userManager;
        this.signManager = signManager;
        this.authService = authService;
        this.emailService = emailService;
    }

    public IActionResult Register()
    {
        return View();
    }

    [HttpGet("Account/Login")]
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

        var confirmationLink = Url.PageLink("/Account/ConfirmEmail", values: new { userId = identity.Id, token = confirmationToken });

        await emailService.SendEmailConfirmationAsync("henriquesigor@yahoo.com.br", identity.Email, confirmationLink);

        return RedirectToAction(nameof(Login));
    }    

    public async Task<IActionResult> ConfirmEmail([FromQuery] string userId, [FromQuery] string token)
    {
        var user = await userManager.FindByIdAsync(userId);

        if (user == null) return NotFound("Usuário não encontrado");

        var emailConfirmation = await userManager.ConfirmEmailAsync(user, token);

        if (!emailConfirmation.Succeeded) return NotFound("Erro ao confirmar e-mail");        

        return View(nameof(ConfirmEmail), "E-mail confirmado com sucesso");
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