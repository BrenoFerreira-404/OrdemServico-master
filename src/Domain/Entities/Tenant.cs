using Domain.Enums;
using Domain.Exceptions;

namespace Domain.Entities;

/// <summary>
/// Entidade que representa um tenant (empresa-cliente) no SaaS.
/// SuperAdmin é o dono da plataforma — não pertence a um tenant.
/// </summary>
public sealed class Tenant
{
    public Guid Id { get; private set; }
    public string Nome { get; private set; } = string.Empty;
    public string Slug { get; private set; } = string.Empty;
    public string? Documento { get; private set; }
    public string Email { get; private set; } = string.Empty;
    public string? Telefone { get; private set; }
    public string? Endereco { get; private set; }
    public string? Cidade { get; private set; }
    public string? Estado { get; private set; }
    public string? LogoUrl { get; private set; }
    public PlanoTenant PlanoAtual { get; private set; }
    public bool Ativo { get; private set; }
    public string? MotivoDesativacao { get; private set; }
    public DateTime? DataExpiracao { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }

    private Tenant() { }

    /// <summary>
    /// Factory method para criação validada de Tenant.
    /// Gera automaticamente o Slug a partir do Nome.
    /// </summary>
    public static Tenant Criar(
        string nome,
        string email,
        string? documento = null,
        string? telefone = null,
        string? endereco = null,
        string? cidade = null,
        string? estado = null,
        PlanoTenant plano = PlanoTenant.Free)
    {
        if (string.IsNullOrWhiteSpace(nome))
            throw new ArgumentException("O nome do tenant é obrigatório.", nameof(nome));

        if (string.IsNullOrWhiteSpace(email))
            throw new ArgumentException("O email do tenant é obrigatório.", nameof(email));

        var slug = GerarSlug(nome);
        var agora = DateTime.UtcNow;

        return new Tenant
        {
            Id = Guid.NewGuid(),
            Nome = nome,
            Slug = slug,
            Documento = documento,
            Email = email,
            Telefone = telefone,
            Endereco = endereco,
            Cidade = cidade,
            Estado = estado,
            PlanoAtual = plano,
            Ativo = true,
            CreatedAt = agora,
            UpdatedAt = agora
        };
    }

    /// <summary>
    /// Gera um slug URL-safe a partir do nome: lowercase, sem acentos, espaços viram hífens.
    /// </summary>
    private static string GerarSlug(string nome)
    {
        if (string.IsNullOrWhiteSpace(nome))
            return string.Empty;

        var texto = nome.Trim().ToLower(System.Globalization.CultureInfo.InvariantCulture);

        // Remover acentos
        var bytes = System.Text.Encoding.GetEncoding("iso-8859-8").GetBytes(texto);
        var resultado = System.Text.Encoding.UTF8.GetString(bytes);

        // Remover caracteres inválidos, converter espaços em hífens
        var slug = System.Text.RegularExpressions.Regex.Replace(resultado, @"[^\w\s\-]", string.Empty);
        slug = System.Text.RegularExpressions.Regex.Replace(slug, @"[\s_-]+", "-");
        slug = slug.Trim('-');

        return slug;
    }

    /// <summary>
    /// Suspende o tenant, impedindo que seus usuários acessem o sistema.
    /// </summary>
    public void Suspender(string motivo)
    {
        if (string.IsNullOrWhiteSpace(motivo))
            throw new ArgumentException("O motivo da suspensão é obrigatório.", nameof(motivo));

        Ativo = false;
        MotivoDesativacao = motivo;
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Reativa um tenant previamente suspenso.
    /// </summary>
    public void Reativar()
    {
        Ativo = true;
        MotivoDesativacao = null;
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Altera o plano de assinatura do tenant.
    /// </summary>
    public void AlterarPlano(PlanoTenant novoPlano)
    {
        if (PlanoAtual == novoPlano)
            return;

        PlanoAtual = novoPlano;
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Renova a data de expiração do tenant (para planos com trial ou vencimento).
    /// </summary>
    public void RenovarAte(DateTime novaExpiracao)
    {
        if (novaExpiracao <= DateTime.UtcNow)
            throw new ArgumentException("A data de expiração deve ser futura.", nameof(novaExpiracao));

        DataExpiracao = novaExpiracao;
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Atualiza os dados do tenant (exceto TenantId, Slug e datas de criação).
    /// </summary>
    public void AtualizarDados(
        string nome,
        string? documento,
        string email,
        string? telefone,
        string? endereco,
        string? cidade,
        string? estado)
    {
        if (string.IsNullOrWhiteSpace(nome))
            throw new ArgumentException("O nome do tenant é obrigatório.", nameof(nome));

        if (string.IsNullOrWhiteSpace(email))
            throw new ArgumentException("O email do tenant é obrigatório.", nameof(email));

        Nome = nome;
        Documento = documento;
        Email = email;
        Telefone = telefone;
        Endereco = endereco;
        Cidade = cidade;
        Estado = estado;
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Verifica se o tenant está expirado.
    /// </summary>
    public bool EstaExpirado() => DataExpiracao.HasValue && DataExpiracao.Value < DateTime.UtcNow;

    /// <summary>
    /// Verifica se o tenant pode ser usado (ativo e não expirado).
    /// </summary>
    public bool PodeSerUsado() => Ativo && !EstaExpirado();
}
