using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using CRM.Domain.Entities;

namespace CRM.Infrastructure.EntitiesConfiguration
{
    public class LeadConfiguration : IEntityTypeConfiguration<Lead>
    {
        public void Configure(EntityTypeBuilder<Lead> builder)
        {
            // Definindo a chave primária
            builder.HasKey(l => l.LeadID);

            // Definindo a propriedade LeadID como obrigatória
            builder.Property(l => l.LeadID)
                   .IsRequired();

            // Definindo as propriedades como obrigatórias e com restrições de tamanho
            builder.Property(l => l.FullName)
                   .IsRequired(false)
                   .HasMaxLength(200);

            builder.Property(l => l.Email)
                   .IsRequired(false)
                   .HasMaxLength(100);

            builder.Property(l => l.Telephone)
                   .IsRequired(false)
                   .HasMaxLength(20);

            // Definindo as propriedades FirstName e LastName como opcionais
            builder.Property(l => l.FirstName)
                   .IsRequired(false)
                   .HasMaxLength(100);

            builder.Property(l => l.LastName)
                   .IsRequired(false)
                   .HasMaxLength(100);

            // Definindo o nome da tabela
            builder.ToTable("Leads");
        }
    }
}