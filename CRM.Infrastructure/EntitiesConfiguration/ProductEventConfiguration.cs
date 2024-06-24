using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using CRM.Domain.Entities;

namespace CRM.Infrastructure.EntitiesConfiguration;
public class ProductEventConfiguration : IEntityTypeConfiguration<ProductEvent>
{
    public void Configure(EntityTypeBuilder<ProductEvent> builder)
    {
        // Definir a chave primária composta
        builder.HasKey(pe => new { pe.ProductID, pe.EventID });

        // Configurar o relacionamento com a entidade Product
        builder.HasOne(pe => pe.Product)
            .WithMany(p => p.ProductEvents)
            .HasForeignKey(pe => pe.ProductID)
            .OnDelete(DeleteBehavior.Cascade);

        // Configurar o relacionamento com a entidade Event
        builder.HasOne(pe => pe.Event)
            .WithMany(e => e.ProductEvents)
            .HasForeignKey(pe => pe.EventID)
            .OnDelete(DeleteBehavior.Cascade);

        // Nome da tabela
        builder.ToTable("ProductEvents");
    }
}