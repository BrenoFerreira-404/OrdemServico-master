namespace Domain.Entities;

/// <summary>
/// Entidade filha para histórico e registros internos do laboratório (não deve sair no PDF final).
/// </summary>
public sealed class OrdemServicoAnotacao
{
    public Guid Id { get; private set; }
    public Guid OrdemServicoId { get; private set; }

    public string Texto { get; private set; } = string.Empty;
    public Guid AutorId { get; private set; }
    public string AutorNome { get; private set; } = string.Empty;

    public DateTime CreatedAt { get; private set; }

    private OrdemServicoAnotacao() { }

    internal static OrdemServicoAnotacao Criar(Guid ordemServicoId, string texto, Guid autorId, string autorNome)
    {
        if (ordemServicoId == Guid.Empty)
            throw new ArgumentException("A Ordem de Serviço deve ser informada.", nameof(ordemServicoId));

        if (string.IsNullOrWhiteSpace(texto))
            throw new ArgumentException("O texto da anotação não pode ser vazio.", nameof(texto));

        if (autorId == Guid.Empty)
            throw new ArgumentException("O autor da anotação é obrigatório.", nameof(autorId));

        if (string.IsNullOrWhiteSpace(autorNome))
            throw new ArgumentException("O nome do autor é obrigatório.", nameof(autorNome));

        return new OrdemServicoAnotacao
        {
            Id = Guid.NewGuid(),
            OrdemServicoId = ordemServicoId,
            Texto = texto,
            AutorId = autorId,
            AutorNome = autorNome,
            CreatedAt = DateTime.UtcNow
        };
    }
}
