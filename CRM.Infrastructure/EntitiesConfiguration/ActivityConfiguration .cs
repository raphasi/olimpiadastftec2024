using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using CRM.Domain.Entities;

namespace CRM.Infrastructure.EntitiesConfiguration
{
    public class ActivityConfiguration : IEntityTypeConfiguration<Activity>
    {
        public void Configure(EntityTypeBuilder<Activity> builder)
        {
            // Definindo a chave primária
            builder.HasKey(a => a.ActivityID);

            // Definindo a propriedade ActivityID como obrigatória
            builder.Property(a => a.ActivityID)
                   .IsRequired();

            // Definindo a propriedade OpportunityID como opcional
            builder.Property(a => a.OpportunityID)
                   .IsRequired(false);

            // Definindo a propriedade Type como opcional
            builder.Property(a => a.Type)
                   .IsRequired(false)
                   .HasMaxLength(100);

            // Definindo a propriedade Description como opcional
            builder.Property(a => a.Description)
                   .IsRequired(false)
                   .HasMaxLength(500);

            // Configurando o relacionamento com a entidade Opportunity
            builder.HasOne(a => a.Opportunity)
                   .WithMany(o => o.Activities)
                   .HasForeignKey(a => a.OpportunityID)
                   .OnDelete(DeleteBehavior.NoAction);

            // Definindo o nome da tabela
            builder.ToTable("Activities");
        }
    }
}