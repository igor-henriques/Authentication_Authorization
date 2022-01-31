namespace Auth.Services.Interfaces;

public interface IAuthService
{
    Task<Token> Authenticate(Credential credential, CancellationToken token = default);
}