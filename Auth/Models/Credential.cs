namespace Auth.Models;

public record Credential
{
    [Required]
    public string Username { get; set; }

    [Required]
    [DataType(DataType.Password)]
    public string Password { get; set; }

    [Display(Name = "Manter Online")]
    public bool RememberMe { get; set; }
}