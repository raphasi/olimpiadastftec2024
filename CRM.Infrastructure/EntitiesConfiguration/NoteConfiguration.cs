using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using CRM.Domain.Entities;

namespace CRM.Infrastructure.EntitiesConfiguration
{
    public class NoteConfiguration : IEntityTypeConfiguration<Note>
    {
        public void Configure(EntityTypeBuilder<Note> builder)
        {
            // Definindo a chave primária
            builder.HasKey(n => n.NoteID);

            // Definindo a propriedade NoteID como obrigatória
            builder.Property(n => n.NoteID)
                   .IsRequired();

            // Definindo as propriedades como opcionais
            builder.Property(n => n.CustomerID)
                   .IsRequired(false);

            builder.Property(n => n.ActivityID)
                   .IsRequired(false);

            builder.Property(n => n.OpportunityID)
                   .IsRequired(false);

            builder.Property(n => n.Content)
                   .IsRequired(false)
                   .HasMaxLength(1000);

            // Configurando o relacionamento com a entidade Customer
            builder.HasOne(n => n.Customer)
                   .WithMany(c => c.Notes)
                   .HasForeignKey(n => n.CustomerID)
                   .OnDelete(DeleteBehavior.NoAction);

            // Configurando o relacionamento com a entidade Opportunity
            builder.HasOne(n => n.Opportunities)
                   .WithMany(o => o.Notes)
                   .HasForeignKey(n => n.OpportunityID)
                   .OnDelete(DeleteBehavior.NoAction);

            // Configurando o relacionamento com a entidade Activity
            builder.HasOne(n => n.Activities)
                   .WithMany(a => a.Notes)
                   .HasForeignKey(n => n.ActivityID)
                   .OnDelete(DeleteBehavior.NoAction);

            // Definindo o nome da tabela
            builder.ToTable("Notes");
        }
    }
}