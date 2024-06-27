using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using CRM.Domain.Entities;

namespace CRM.Infrastructure.EntitiesConfiguration
{
    public class EventConfiguration : IEntityTypeConfiguration<Event>
    {
        public void Configure(EntityTypeBuilder<Event> builder)
        {
            // Definindo a chave primária
            builder.HasKey(e => e.EventID);

            // Definindo a propriedade EventID como obrigatória
            builder.Property(e => e.EventID)
                   .IsRequired();

            // Definindo as propriedades como opcionais
            builder.Property(e => e.ProductID)
                   .IsRequired(false);

            builder.Property(e => e.Name)
                   .IsRequired(false)
                   .HasMaxLength(200);

            builder.Property(e => e.Description)
                   .IsRequired(false)
                   .HasMaxLength(500);

            builder.Property(e => e.EventDate)
                   .IsRequired(false);
            // Definindo a propriedade ImageUrl como opcional e com tamanho máximo
            builder.Property(p => p.ImageUrl)
                   .HasMaxLength(500);

            builder.Property(e => e.Location)
                   .IsRequired(false)
                   .HasMaxLength(200);

            builder.Property(e => e.TicketPrice)
                   .IsRequired(false)
                   .HasColumnType("decimal(18,2)");

            // Configurando o relacionamento com a entidade Product
            builder.HasOne(e => e.Product)
                   .WithMany(p => p.Events)
                   .HasForeignKey(e => e.ProductID)
                   .OnDelete(DeleteBehavior.NoAction);

            // Configurando o relacionamento com a entidade Quote
            builder.HasMany(e => e.Quotes)
                   .WithOne(q => q.Event)
                   .HasForeignKey(q => q.EventID)
                   .OnDelete(DeleteBehavior.NoAction);

            // Definindo o nome da tabela
            builder.ToTable("Events");
        }
    }
}