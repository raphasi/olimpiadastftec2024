using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using CRM.Domain.Entities;

namespace CRM.Infrastructure.EntitiesConfiguration;

public class OpportunityConfiguration : IEntityTypeConfiguration<Opportunity>
{
    public void Configure(EntityTypeBuilder<Opportunity> builder)
    {
        // Definindo a chave primária
        builder.HasKey(o => o.OpportunityID);

        // Definindo a propriedade OpportunityID como obrigatória
        builder.Property(o => o.OpportunityID)
               .IsRequired();

        // Definindo as propriedades como obrigatórias ou opcionais
        builder.Property(o => o.CustomerID)
               .IsRequired(false);

        // Definindo as propriedades como obrigatórias e com restrições de tamanho
        builder.Property(l => l.Name)
               .IsRequired(false)
               .HasMaxLength(200);

        builder.Property(o => o.LeadID)
               .IsRequired(false);

        builder.Property(o => o.Description)
               .IsRequired()
               .HasMaxLength(500);

        builder.Property(o => o.EstimatedValue)
               .IsRequired(false)
               .HasColumnType("decimal(18,2)");

        builder.Property(o => o.ExpectedCloseDate)
               .IsRequired(false);

        // Configurando o relacionamento com a entidade Customer
        builder.HasOne(o => o.Customer)
               .WithMany(c => c.Opportunities)
               .HasForeignKey(o => o.CustomerID)
               .OnDelete(DeleteBehavior.SetNull);

        // Configurando o relacionamento com a entidade Lead
        builder.HasOne(o => o.Lead)
               .WithMany(l => l.Opportunities)
               .HasForeignKey(o => o.LeadID)
               .OnDelete(DeleteBehavior.SetNull);

        // Configurando o relacionamento com a entidade Order
        builder.HasMany(o => o.Orders)
               .WithOne(order => order.Opportunity)
               .HasForeignKey(order => order.OpportunityID)
               .OnDelete(DeleteBehavior.NoAction);

        // Configurando o relacionamento com a entidade Quote
        builder.HasMany(o => o.Quotes)
               .WithOne(quote => quote.Opportunity)
               .HasForeignKey(quote => quote.OpportunityID)
               .OnDelete(DeleteBehavior.NoAction);

        // Configurando o relacionamento com a entidade Activity
        builder.HasMany(o => o.Activities)
               .WithOne(activity => activity.Opportunity)
               .HasForeignKey(activity => activity.OpportunityID)
               .OnDelete(DeleteBehavior.NoAction);

        // Configurando o relacionamento com a entidade Note
        builder.HasMany(o => o.Notes)
               .WithOne(note => note.Opportunities)
               .HasForeignKey(note => note.OpportunityID)
               .OnDelete(DeleteBehavior.NoAction);

        // Definindo o nome da tabela
        builder.ToTable("Opportunities");
    }
}