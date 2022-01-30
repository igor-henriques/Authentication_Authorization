namespace WebApp.Services;

public class AuthService : IAuthService
{
    private const string Resource = "Auth";
    private readonly HttpClient client;

    public AuthService(HttpClient client)
    {
        this.client = client;
    }

    public async Task<Token> Authenticate(Credential credential, CancellationToken token = default)
    {
        var response = await client.PostAsJsonAsync($"{client.BaseAddress}{Resource}", credential, token);

        response.EnsureSuccessStatusCode();

        var bearerToken = JsonConvert.DeserializeObject<Token>(await response.Content.ReadAsStringAsync());

        return bearerToken;
    }
}