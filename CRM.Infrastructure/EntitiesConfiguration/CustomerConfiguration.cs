using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using CRM.Domain.Entities;

namespace CRM.Infrastructure.EntitiesConfiguration
{
    public class CustomerConfiguration : IEntityTypeConfiguration<Customer>
    {
        public void Configure(EntityTypeBuilder<Customer> builder)
        {
            // Definindo a chave primária
            builder.HasKey(c => c.CustomerID);

            // Definindo a propriedade CustomerID como obrigatória
            builder.Property(c => c.CustomerID)
                   .IsRequired();

            // Definindo as propriedades como opcionais
            builder.Property(c => c.FullName)
                   .IsRequired(false)
                   .HasMaxLength(200);

            builder.Property(c => c.FirstName)
                   .IsRequired(false)
                   .HasMaxLength(100);

            builder.Property(c => c.LastName)
                   .IsRequired(false)
                   .HasMaxLength(100);

            builder.Property(c => c.Email)
                   .IsRequired(false)
                   .HasMaxLength(100);

            builder.Property(c => c.Telephone)
                   .IsRequired(false)
                   .HasMaxLength(20);

            builder.Property(c => c.Address1)
                   .IsRequired(false)
                   .HasMaxLength(200);

            builder.Property(c => c.Address_PostalCode)
                   .IsRequired(false)
                   .HasMaxLength(20);

            builder.Property(c => c.Address_Country)
                   .IsRequired(false)
                   .HasMaxLength(100);

            builder.Property(c => c.Address_State)
                   .IsRequired(false)
                   .HasMaxLength(100);

            builder.Property(c => c.Address_City)
                   .IsRequired(false)
                   .HasMaxLength(100);

            builder.Property(c => c.Address_Adjunct)
                   .IsRequired(false)
                   .HasMaxLength(100);

            builder.Property(c => c.TypeLead)
                   .IsRequired(false);

            builder.Property(c => c.CPF)
                   .IsRequired(false);

            builder.Property(c => c.CNPJ)
                   .IsRequired(false);

            // Configurando o relacionamento com a entidade Opportunity
            builder.HasMany(c => c.Opportunities)
                   .WithOne(o => o.Customer)
                   .HasForeignKey(o => o.CustomerID)
                   .OnDelete(DeleteBehavior.NoAction);

            // Definindo o nome da tabela
            builder.ToTable("Customers");
        }
    }
}