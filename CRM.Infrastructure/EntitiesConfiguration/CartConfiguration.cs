using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using CRM.Domain.Entities;

namespace CRM.Infrastructure.EntitiesConfiguration
{
    public class CartConfiguration : IEntityTypeConfiguration<Cart>
    {
        public void Configure(EntityTypeBuilder<Cart> builder)
        {
            // Definindo a chave primária
            builder.HasKey(c => c.CartID);

            // Definindo a propriedade CartID como obrigatória
            builder.Property(c => c.CartID)
                   .IsRequired();

            // Definindo a propriedade UserID como obrigatória
            builder.Property(c => c.UserID)
                   .IsRequired();

            // Configurando o relacionamento com a entidade CartItem
            builder.HasMany(c => c.CartItems)
                   .WithOne(ci => ci.Cart)
                   .HasForeignKey(ci => ci.CartID)
                   .OnDelete(DeleteBehavior.NoAction);

            // Configurando propriedades herdadas
            builder.Property(c => c.CreatedBy).IsRequired(false);
            builder.Property(c => c.ModifiedBy).IsRequired(false);
            builder.Property(c => c.CreatedOn).IsRequired(false);
            builder.Property(c => c.ModifiedOn).IsRequired(false);
            builder.Property(c => c.StatusCode).IsRequired(false);

            // Definindo o nome da tabela
            builder.ToTable("Carts");
        }
    }
}