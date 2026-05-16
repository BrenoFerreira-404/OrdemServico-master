namespace Domain.Interfaces;

/// <summary>
/// Marca entidades isoladas por tenant no modelo de dados.
/// </summary>
public interface ITenantEntidade
{
    Guid TenantId { get; }
}
