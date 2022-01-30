namespace API.Controllers;

[Authorize]
public class AddressController : Controller
{
    private readonly IAddressService addressService;
    private readonly IValidator<string> cepValidator;

    public AddressController(IAddressService addressService, IValidator<string> cepValidator)
    {
        this.addressService = addressService;
        this.cepValidator = cepValidator;
    }

    [HttpGet("GetCEP")]
    public async Task<IActionResult> GetCEP([FromQuery] string cep, CancellationToken token = default)
    {
        var validation = await cepValidator.ValidateAsync(cep);

        if (!validation.IsValid) return BadRequest(validation.Errors.Select(x => x.ErrorMessage));

        var response = await addressService.GetCepAsync(cep, token);

        if (response is default(Address)) return NotFound("Não há endereço cadastrado com o CEP informado.");

        return Ok(response);
    }
}