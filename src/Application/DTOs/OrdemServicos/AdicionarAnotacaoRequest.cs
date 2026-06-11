namespace Application.DTOs.OrdemServicos;

public record AdicionarAnotacaoRequest(
    string Texto,
    Guid UsuarioId,
    string UsuarioNome
);
