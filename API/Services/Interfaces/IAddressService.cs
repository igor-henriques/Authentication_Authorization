namespace API.Services.Interfaces;

public interface IAddressService
{
    Task<Address> GetCepAsync(string cep, CancellationToken token = default);
}