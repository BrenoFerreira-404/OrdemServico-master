using Domain.Entities;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

/// <summary>
/// Configuração EF Core para a entidade Tenant.
/// </summary>
public sealed class TenantConfiguration : IEntityTypeConfiguration<Tenant>
{
    public void Configure(EntityTypeBuilder<Tenant> builder)
    {
        builder.ToTable("tenants");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Nome)
            .IsRequired()
            .HasMaxLength(150);

        builder.Property(x => x.Slug)
            .IsRequired()
            .HasMaxLength(100);

        builder.HasIndex(x => x.Slug).IsUnique();

        builder.Property(x => x.Documento)
            .HasMaxLength(20);

        builder.Property(x => x.Email)
            .IsRequired()
            .HasMaxLength(256);

        builder.Property(x => x.Telefone)
            .HasMaxLength(20);

        builder.Property(x => x.Endereco)
            .HasMaxLength(250);

        builder.Property(x => x.Cidade)
            .HasMaxLength(100);

        builder.Property(x => x.Estado)
            .HasMaxLength(2);

        builder.Property(x => x.LogoUrl)
            .HasMaxLength(500);

        builder.Property(x => x.PlanoAtual)
            .HasConversion<int>();

        builder.Property(x => x.Ativo);

        builder.Property(x => x.MotivoDesativacao)
            .HasMaxLength(250);

        builder.Property(x => x.DataExpiracao)
            .IsRequired(false);

        builder.Property(x => x.CreatedAt);
        builder.Property(x => x.UpdatedAt);
    }
}
