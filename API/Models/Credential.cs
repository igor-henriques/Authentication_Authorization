namespace API.Models;

public record Credential
{
    [Required]
    public string Username { get; set; }

    [Required]
    [DataType(DataType.Password)]
    public string Password { get; set; }

    public static Credential MokedCredential
    {
        get { return new Credential { Username = "admin", Password = "1" }; }
    }
}