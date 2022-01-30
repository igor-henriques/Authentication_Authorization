namespace API.Models;

public record Address
{
    public string CEP { get; init; }
    public string Logradouro { get; init; }
    public string Complemento { get; init; }
    public string Bairro { get; init; }
    public string Localidade { get; init; }
    public string UF { get; init; }
    public string DDD { get; init; }
}