namespace API.Services;

public class AddressService : IAddressService
{
    private readonly HttpClient _httpClient;
    private const string baseAddress = "https://viacep.com.br/ws/{0}/json";

    public AddressService(HttpClient httpClient)
    {
        this._httpClient = httpClient;
    }

    public async Task<Address> GetCepAsync(string cep, CancellationToken token = default)
    {
        var url = string.Format(baseAddress, cep);

        var response = await _httpClient
            .GetFromJsonAsync<Address>(url, token)
            .ConfigureAwait(false);

        if (response.CEP is null) return default(Address);

        return response;
    }
}