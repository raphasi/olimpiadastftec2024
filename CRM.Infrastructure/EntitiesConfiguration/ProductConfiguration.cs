using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using CRM.Domain.Entities;

namespace CRM.Infrastructure.EntitiesConfiguration;

public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        // Definindo a chave primária
        builder.HasKey(p => p.ProductID);

        // Definindo a propriedade ProductID como obrigatória
        builder.Property(p => p.ProductID)
               .IsRequired();

        // Definindo a propriedade Name como obrigatória e com tamanho máximo
        builder.Property(p => p.Name)
               .IsRequired(false)
               .HasMaxLength(200);

        // Definindo a propriedade Description como opcional e com tamanho máximo
        builder.Property(p => p.Description)
               .HasMaxLength(1000);

        // Definindo a propriedade Price como obrigatória
        builder.Property(p => p.Price)
               .IsRequired(false)
               .HasColumnType("decimal(18,2)");

        // Definindo a propriedade ImageUrl como opcional e com tamanho máximo
        builder.Property(p => p.ImageUrl)
               .HasMaxLength(500);

        // Definindo a propriedade Inventory como obrigatória
        builder.Property(p => p.Inventory)
               .IsRequired(false);

        // Configurando o relacionamento com a entidade Quote
        builder.HasMany(p => p.Quotes)
               .WithOne(q => q.Product)
               .HasForeignKey(q => q.ProductID)
               .OnDelete(DeleteBehavior.NoAction);

        // Configurando o relacionamento com a entidade Event
        builder.HasMany(p => p.Events)
               .WithOne(e => e.Product)
               .HasForeignKey(e => e.ProductID)
               .OnDelete(DeleteBehavior.NoAction);

        // Definindo o nome da tabela
        builder.ToTable("Products");
    }
}