namespace API.Validators;

public class CredentialValidator : AbstractValidator<Credential>
{
    public CredentialValidator()
    {
        RuleFor(x => x.Password)
            .NotEmpty()
            .Length(8, 32);

        RuleFor(x => x.Username)
            .NotEmpty()
            .Length(3, 30);
    }
}