using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public sealed class OrdemServicoAnotacaoConfiguration : IEntityTypeConfiguration<OrdemServicoAnotacao>
{
    public void Configure(EntityTypeBuilder<OrdemServicoAnotacao> builder)
    {
        builder.ToTable("os_anotacoes");
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Texto).IsRequired().HasColumnType("text");
        builder.Property(x => x.AutorId).IsRequired().HasColumnName("autor_id");
        builder.Property(x => x.AutorNome).IsRequired().HasMaxLength(100).HasColumnName("autor_nome");

        builder.HasOne<OrdemServico>()
            .WithMany(o => o.Anotacoes)
            .HasForeignKey(x => x.OrdemServicoId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
