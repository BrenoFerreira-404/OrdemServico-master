using Domain.Exceptions;

namespace Domain.Entities;

/// <summary>
/// Representa uma empresa/oficina no modelo SaaS multi-tenant.
/// </summary>
public sealed class Tenant
{
    public Guid Id { get; private set; }
    public string Nome { get; private set; } = string.Empty;
    public string Slug { get; private set; } = string.Empty;
    public bool Ativo { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }

    private Tenant() { }

    public static Tenant Criar(string nome, string slug)
    {
        if (string.IsNullOrWhiteSpace(nome))
            throw new ArgumentException("O nome do tenant e obrigatorio.", nameof(nome));

        if (string.IsNullOrWhiteSpace(slug))
            throw new ArgumentException("O slug do tenant e obrigatorio.", nameof(slug));

        var slugNormalizado = slug.Trim().ToLowerInvariant();
        if (slugNormalizado.Length < 2)
            throw new ArgumentException("O slug deve ter ao menos 2 caracteres.", nameof(slug));

        var agora = DateTime.UtcNow;

        return new Tenant
        {
            Id = Guid.NewGuid(),
            Nome = nome.Trim(),
            Slug = slugNormalizado,
            Ativo = true,
            CreatedAt = agora,
            UpdatedAt = agora
        };
    }

    public static Tenant CriarComId(Guid id, string nome, string slug)
    {
        var tenant = Criar(nome, slug);
        if (id == Guid.Empty)
            throw new ArgumentException("O id do tenant e invalido.", nameof(id));

        return new Tenant
        {
            Id = id,
            Nome = tenant.Nome,
            Slug = tenant.Slug,
            Ativo = tenant.Ativo,
            CreatedAt = tenant.CreatedAt,
            UpdatedAt = tenant.UpdatedAt
        };
    }

    public void Desativar()
    {
        if (!Ativo)
            throw new DomainException("Tenant ja esta desativado.");

        Ativo = false;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Reativar()
    {
        Ativo = true;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Atualizar(string nome)
    {
        if (string.IsNullOrWhiteSpace(nome))
            throw new ArgumentException("O nome do tenant e obrigatorio.", nameof(nome));

        Nome = nome.Trim();
        UpdatedAt = DateTime.UtcNow;
    }
}
