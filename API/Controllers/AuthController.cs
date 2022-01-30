namespace API.Controllers;

[ApiController]
[Route("[controller]")]
public class AuthController : Controller
{
    private readonly IValidator<Credential> credentialValidator;
    private readonly IConfiguration configuration;

    public AuthController(IValidator<Credential> credentialValidator, IConfiguration configurations)
    {
        this.credentialValidator = credentialValidator;
        this.configuration = configurations;
    }

    [HttpPost]
    public async Task<IActionResult> Authorize([FromBody] Credential credential)
    {
        var validationResult = await credentialValidator.ValidateAsync(credential);

        if (!validationResult.IsValid) return BadRequest(validationResult.Errors.Select(x => x.ErrorMessage));

        var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, "admin"),
                new Claim(ClaimTypes.Email, "henriquesigor@yahoo.com.br"),
                new Claim("Department", "HR"),
                new Claim("EmploymentDate", "2020-01-28"),
                new Claim("Manager", "true")
            };

        var expiresAt = DateTime.UtcNow.AddMinutes(10);

        return Ok(new
        {
            token = GenerateToken(claims, expiresAt),
            expires_at = expiresAt
        });
    }

    private string GenerateToken(IEnumerable<Claim> claims, DateTime expiresAt)
    {
        var key = Encoding.ASCII.GetBytes(configuration.GetValue<string>("SecretKey"));

        var jwt = new JwtSecurityToken(
            claims: claims,
            notBefore: DateTime.UtcNow,
            expires: expiresAt,
            signingCredentials: new SigningCredentials(
                new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature));

        return new JwtSecurityTokenHandler().WriteToken(jwt);
    }
}