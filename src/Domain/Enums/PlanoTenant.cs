namespace Domain.Enums;

/// <summary>
/// Planos de assinatura disponiveis para tenants SaaS.
/// Cada plano oferece diferentes limites de recursos e features.
/// </summary>
public enum PlanoTenant
{
    Free = 0,
    Starter = 1,
    Pro = 2,
    Business = 3,
    Enterprise = 4
}
