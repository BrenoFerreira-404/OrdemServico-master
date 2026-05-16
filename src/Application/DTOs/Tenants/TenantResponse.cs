namespace Application.DTOs.Tenants;

public sealed record TenantResponse(
    Guid Id,
    string Nome,
    string Slug,
    bool Ativo,
    DateTime CreatedAt,
    DateTime UpdatedAt);
