namespace API.Validators;

public class CepValidator : AbstractValidator<string>
{    
    public CepValidator()
    {
        RuleFor(x => x)
            .NotNull()
            .NotEmpty()
            .Length(8);
    }
}