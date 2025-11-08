using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configuration;
internal sealed class ClientConfiguration : IEntityTypeConfiguration<Client>
{
    public void Configure(EntityTypeBuilder<Client> builder)
    {
        builder.HasKey(e => e.Id);

        builder.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(255);

        builder.Property(e => e.Email)
                .IsRequired()
                .HasMaxLength(255);

        builder.Property(e => e.PhoneNumber)
                .IsRequired()
                .HasMaxLength(20);

        builder.Property(e => e.CreatedAt)
                .IsRequired();

        builder.Property(e => e.UpdatedAt)
                .IsRequired(false);
    }
}