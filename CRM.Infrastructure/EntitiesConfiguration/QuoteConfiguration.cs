using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using CRM.Domain.Entities;

namespace CRM.Infrastructure.EntitiesConfiguration
{
    public class QuoteConfiguration : IEntityTypeConfiguration<Quote>
    {
        public void Configure(EntityTypeBuilder<Quote> builder)
        {
            // Definindo a chave primária
            builder.HasKey(q => q.QuoteID);

            // Definindo a propriedade QuoteID como obrigatória
            builder.Property(q => q.QuoteID)
                   .IsRequired();

            // Definindo as propriedades como obrigatórias e com restrições de tamanho
            builder.Property(l => l.Name)
                   .IsRequired(false)
                   .HasMaxLength(200);

            // Definindo a propriedade OpportunityID como obrigatória
            builder.Property(q => q.OpportunityID)
                   .IsRequired(false);

            // Definindo a propriedade ProductID como obrigatória
            builder.Property(q => q.ProductID)
                   .IsRequired(false);

            // Definindo a propriedade PriceLevelID como obrigatória
            builder.Property(q => q.PriceLevelID)
                   .IsRequired(false);

            // Definindo a propriedade EventID como obrigatória
            builder.Property(q => q.EventID)
                   .IsRequired(false);

            // Definindo a propriedade CustomerID como obrigatória
            builder.Property(q => q.CustomerID)
                   .IsRequired(false);

            // Definindo a propriedade LeadID como obrigatória
            builder.Property(q => q.LeadID)
                   .IsRequired(false);

            // Definindo a propriedade Quantity como obrigatória
            builder.Property(q => q.Quantity)
                   .IsRequired(false);

            // Definindo a propriedade Discount como obrigatória
            builder.Property(q => q.Discount)
                   .IsRequired(false)
                   .HasColumnType("decimal(18,2)");

            // Definindo a propriedade TotalPrice como obrigatória
            builder.Property(q => q.TotalPrice)
                   .IsRequired(false)
                   .HasColumnType("decimal(18,2)");

            // Configurando o relacionamento com a entidade Opportunity
            builder.HasOne(q => q.Opportunity)
                   .WithMany(o => o.Quotes)
                   .HasForeignKey(q => q.OpportunityID)
                   .OnDelete(DeleteBehavior.NoAction);

            // Configurando o relacionamento com a entidade Product
            builder.HasOne(q => q.Product)
                   .WithMany(p => p.Quotes)
                   .HasForeignKey(q => q.ProductID)
                   .OnDelete(DeleteBehavior.NoAction);

            // Configurando o relacionamento com a entidade PriceLevel
            builder.HasOne(q => q.PriceLevel)
                   .WithMany(pl => pl.Quotes)
                   .HasForeignKey(q => q.PriceLevelID)
                   .OnDelete(DeleteBehavior.NoAction);

            // Configurando o relacionamento com a entidade Event
            builder.HasOne(q => q.Event)
                   .WithMany(e => e.Quotes)
                   .HasForeignKey(q => q.EventID)
                   .OnDelete(DeleteBehavior.NoAction);

            // Definindo o nome da tabela
            builder.ToTable("Quotes");
        }
    }
}