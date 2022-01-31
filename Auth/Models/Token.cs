namespace Auth.Models;

public record Token
{
    [JsonProperty("token")]
    public string BearerToken { get; init; }

    [JsonProperty("expires_at")]
    public DateTime ExpiresAt { get; init; }
}