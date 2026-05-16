using Domain.Entities;

namespace Application.DTOs.Tenants.Mappings;

public static class TenantMappings
{
    public static TenantResponse ToResponse(this Tenant tenant) =>
        new(
            Id: tenant.Id,
            Nome: tenant.Nome,
            Slug: tenant.Slug,
            Ativo: tenant.Ativo,
            CreatedAt: tenant.CreatedAt,
            UpdatedAt: tenant.UpdatedAt);
}
