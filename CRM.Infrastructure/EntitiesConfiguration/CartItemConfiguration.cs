using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using CRM.Domain.Entities;

namespace CRM.Infrastructure.EntitiesConfiguration
{
    public class CartItemConfiguration : IEntityTypeConfiguration<CartItem>
    {
        public void Configure(EntityTypeBuilder<CartItem> builder)
        {
            // Definindo a chave primária
            builder.HasKey(ci => ci.CartItemID);

            // Definindo a propriedade CartItemID como obrigatória
            builder.Property(ci => ci.CartItemID)
                   .IsRequired();

            // Definindo a propriedade CartID como obrigatória
            builder.Property(ci => ci.CartID)
                   .IsRequired();

            // Definindo a propriedade ProductID como obrigatória
            builder.Property(ci => ci.ProductID)
                   .IsRequired();

            // Definindo a propriedade Quantity como obrigatória
            builder.Property(ci => ci.Quantity)
                   .IsRequired();

            // Definindo a propriedade UnitPrice como obrigatória
            builder.Property(ci => ci.UnitPrice)
                   .IsRequired()
                   .HasColumnType("decimal(18,2)");

            // Configurando o relacionamento com a entidade Product
            builder.HasOne(ci => ci.Product)
                   .WithMany()
                   .HasForeignKey(ci => ci.ProductID)
                   .OnDelete(DeleteBehavior.Restrict);

            // Configurando propriedades herdadas
            builder.Property(ci => ci.CreatedBy).IsRequired(false);
            builder.Property(ci => ci.ModifiedBy).IsRequired(false);
            builder.Property(ci => ci.CreatedOn).IsRequired(false);
            builder.Property(ci => ci.ModifiedOn).IsRequired(false);
            builder.Property(ci => ci.StatusCode).IsRequired(false);

            // Definindo o nome da tabela
            builder.ToTable("CartItems");
        }
    }
}