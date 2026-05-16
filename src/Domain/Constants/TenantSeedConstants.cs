namespace Domain.Constants;

/// <summary>
/// Identificadores fixos usados no seed e em testes de integracao.
/// </summary>
public static class TenantSeedConstants
{
    public static readonly Guid DemonstracaoId = Guid.Parse("a1000000-0000-4000-8000-000000000001");

    public const string DemonstracaoNome = "Oficina Demonstracao";
    public const string DemonstracaoSlug = "demo";

    public const string AdminDemoEmail = "admin@demo.local";
    public const string AdminDemoSenha = "Demo@123456";

    public const string SuperAdminEmail = "admin@ordemservico.com";
    public const string SuperAdminSenha = "Admin@123456";
}
