using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configuration;
internal sealed class ProviderConfiguration : IEntityTypeConfiguration<Provider>
{
    public void Configure(EntityTypeBuilder<Provider> builder)
    {
        builder.HasKey(e => e.Id);

        builder.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(255);

        builder.Property(e => e.CreatedAt)
                .IsRequired();

        builder.Property(e => e.UpdatedAt)
                .IsRequired(false);
    }
}