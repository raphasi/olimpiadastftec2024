using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using CRM.Domain.Entities;

namespace CRM.Infrastructure.EntitiesConfiguration
{
    public class PriceLevelConfiguration : IEntityTypeConfiguration<PriceLevel>
    {
        public void Configure(EntityTypeBuilder<PriceLevel> builder)
        {
            // Definindo a chave primária
            builder.HasKey(pl => pl.PriceLevelID);

            // Definindo a propriedade PriceLevelID como obrigatória
            builder.Property(pl => pl.PriceLevelID)
                   .IsRequired();

            // Definindo a propriedade LevelName como obrigatória e com tamanho máximo
            builder.Property(pl => pl.LevelName)
                   .IsRequired(false)
                   .HasMaxLength(100);

            // Definindo a propriedade DiscountPercentage como obrigatória
            builder.Property(pl => pl.DiscountPercentage)
                   .IsRequired(false)
                   .HasColumnType("decimal(5,2)");

            // Definindo a propriedade ValueBase como obrigatória
            builder.Property(pl => pl.ValueBase)
                   .IsRequired(false)
                   .HasColumnType("decimal(18,2)");

            // Configurando o relacionamento com a entidade Quote
            builder.HasMany(pl => pl.Quotes)
                   .WithOne(q => q.PriceLevel)
                   .HasForeignKey(q => q.PriceLevelID)
                   .OnDelete(DeleteBehavior.Restrict);

            // Definindo o nome da tabela
            builder.ToTable("PriceLevels");
        }
    }
}