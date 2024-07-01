using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using CRM.Domain.Entities;
using System.Reflection.Emit;

namespace CRM.Infrastructure.EntitiesConfiguration
{
    public class OrderItemConfiguration : IEntityTypeConfiguration<OrderItem>
    {
        public void Configure(EntityTypeBuilder<OrderItem> builder)
        {
            // Definindo a chave primária
            builder.HasKey(oi => oi.OrderItemID);

            // Definindo a propriedade OrderItemID como obrigatória
            builder.Property(oi => oi.OrderItemID)
                   .IsRequired(true);

            // Definindo a propriedade OrderID como obrigatória
            builder.Property(oi => oi.OrderID)
                   .IsRequired(false);

            // Definindo a propriedade ProductID como obrigatória
            builder.Property(oi => oi.ProductID)
                   .IsRequired(false);

            // Definindo a propriedade QuoteID como obrigatória
            builder.Property(o => o.QuoteID)
                   .IsRequired(false);

            // Definindo a propriedade Quantity como obrigatória
            builder.Property(oi => oi.Quantity)
                   .IsRequired(false);

            // Definindo a propriedade UnitPrice como obrigatória
            builder.Property(oi => oi.UnitPrice)
                   .IsRequired(false)
                   .HasColumnType("decimal(18,2)");

            // Definindo a propriedade TotalPrice como obrigatória
            builder.Property(oi => oi.TotalPrice)
                   .IsRequired(false)
                   .HasColumnType("decimal(18,2)");

            // Configurando o relacionamento com a entidade Order
            builder.HasOne(oi => oi.Order)
                   .WithMany(o => o.OrderItems)
                   .HasForeignKey(oi => oi.OrderID)
                   .OnDelete(DeleteBehavior.NoAction);

            // Configurando o relacionamento com a entidade Product
            builder.HasOne(oi => oi.Product)
                   .WithMany(p => p.OrderItems)
                   .HasForeignKey(oi => oi.ProductID)
                   .OnDelete(DeleteBehavior.NoAction);

            // Definindo o nome da tabela
            builder.ToTable("OrderItems");
        }
    }
}