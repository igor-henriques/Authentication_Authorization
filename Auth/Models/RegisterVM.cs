namespace Auth.Models;

public record RegisterVM
{
    [Required]
    [EmailAddress(ErrorMessage = "Endereço de e-mail inválido.")]
    public string Email { get; set; }

    [Required]
    [DataType(DataType.Password)]
    public string Password { get; set; }
}