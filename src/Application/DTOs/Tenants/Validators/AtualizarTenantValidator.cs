using FluentValidation;

namespace Application.DTOs.Tenants.Validators;

public sealed class AtualizarTenantValidator : AbstractValidator<AtualizarTenantRequest>
{
    public AtualizarTenantValidator()
    {
        RuleFor(x => x.Nome)
            .NotEmpty()
            .MaximumLength(150);
    }
}
