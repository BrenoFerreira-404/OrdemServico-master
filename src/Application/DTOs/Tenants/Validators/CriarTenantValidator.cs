using FluentValidation;

namespace Application.DTOs.Tenants.Validators;

public sealed class CriarTenantValidator : AbstractValidator<CriarTenantRequest>
{
    public CriarTenantValidator()
    {
        RuleFor(x => x.Nome)
            .NotEmpty()
            .MaximumLength(150);

        RuleFor(x => x.Slug)
            .NotEmpty()
            .MinimumLength(2)
            .MaximumLength(50)
            .Matches("^[a-z0-9]+(?:-[a-z0-9]+)*$")
            .WithMessage("Slug deve conter apenas letras minusculas, numeros e hifens.");
    }
}
