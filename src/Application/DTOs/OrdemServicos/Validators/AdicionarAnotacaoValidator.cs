using FluentValidation;

namespace Application.DTOs.OrdemServicos.Validators;

public sealed class AdicionarAnotacaoValidator : AbstractValidator<AdicionarAnotacaoRequest>
{
    public AdicionarAnotacaoValidator()
    {
        RuleFor(x => x.Texto)
            .NotEmpty().WithMessage("O texto da anotação é obrigatório.");

        RuleFor(x => x.UsuarioNome)
            .NotEmpty().WithMessage("O nome do usuário é obrigatório.");
    }
}
