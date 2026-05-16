using Domain.Interfaces;

namespace Domain.Entities;

/// <summary>
/// Entidade Cliente que representa o tomador do serviço.
/// Regras: Nome é obrigatório.
/// </summary>
public sealed class Cliente : ITenantEntidade
{
    public Guid Id { get; private set; }
    public Guid TenantId { get; private set; }
    public string Nome { get; private set; } = string.Empty;
    public string? Documento { get; private set; }
    public string? Telefone { get; private set; }
    public string? Email { get; private set; }
    public string? Endereco { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }

    /// <summary>
    /// Construtor privado para o Entity Framework.
    /// </summary>
    private Cliente() { }

    /// <summary>
    /// Factory method para criação validada de Cliente.
    /// </summary>
    public static Cliente Criar(Guid tenantId, string nome, string? documento, string? telefone, string? email, string? endereco)
    {
        if (tenantId == Guid.Empty)
            throw new ArgumentException("O tenant do cliente e obrigatorio.", nameof(tenantId));

        if (string.IsNullOrWhiteSpace(nome))
            throw new ArgumentException("O nome do cliente é obrigatório.", nameof(nome));

        var dataAtual = DateTime.UtcNow;

        return new Cliente
        {
            Id = Guid.NewGuid(),
            TenantId = tenantId,
            Nome = nome,
            Documento = documento,
            Telefone = telefone,
            Email = email,
            Endereco = endereco,
            CreatedAt = dataAtual,
            UpdatedAt = dataAtual
        };
    }

    /// <summary>
    /// Método de negócio para atualização dos dados do cliente.
    /// </summary>
    public void Atualizar(string nome, string? documento, string? telefone, string? email, string? endereco)
    {
        if (string.IsNullOrWhiteSpace(nome))
            throw new ArgumentException("O nome do cliente é obrigatório.", nameof(nome));

        Nome = nome;
        Documento = documento;
        Telefone = telefone;
        Email = email;
        Endereco = endereco;
        UpdatedAt = DateTime.UtcNow;
    }
}
