using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using CRM.Domain.Entities;

namespace CRM.Infrastructure.EntitiesConfiguration
{
    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            // Definindo a chave primária
            builder.HasKey(o => o.OrderID);

            // Definindo a propriedade OrderID como obrigatória
            builder.Property(o => o.OrderID)
                   .IsRequired(true);

            // Definindo a propriedade OpportunityID como obrigatória
            builder.Property(o => o.OpportunityID)
                   .IsRequired(false);

            // Definindo a propriedade QuoteID como obrigatória
            builder.Property(o => o.QuoteID)
                   .IsRequired(false);

            // Definindo a propriedade OrderDate como obrigatória
            builder.Property(o => o.OrderDate)
                   .IsRequired(false);

            // Definindo a propriedade TotalAmount como obrigatória
            builder.Property(o => o.TotalAmount)
                   .IsRequired(false)
                   .HasColumnType("decimal(18,2)");

            // Configurando o relacionamento com a entidade Opportunity
            builder.HasOne(o => o.Opportunity)
                   .WithMany(op => op.Orders)
                   .HasForeignKey(o => o.OpportunityID)
                   .OnDelete(DeleteBehavior.NoAction);

            // Configurando o relacionamento com a entidade OrderItem
            builder.HasMany(o => o.OrderItems)
                   .WithOne(oi => oi.Order)
                   .HasForeignKey(oi => oi.OrderID)
                   .OnDelete(DeleteBehavior.Cascade);

            // Definindo o nome da tabela
            builder.ToTable("Orders");
        }
    }
}