using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configuration;
internal sealed class SessionTypeConfiguration : IEntityTypeConfiguration<SessionType>
{
    public void Configure(EntityTypeBuilder<SessionType> builder)
    {
        builder.HasKey(e => e.Id);

        builder.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(255);

        builder.Property(e => e.CreatedAt)
                .IsRequired();

        builder.Property(e => e.UpdatedAt)
                .IsRequired(false);

        builder.HasMany(e => e.ProviderSessionTypes)
                .WithOne(e => e.SessionType)
                .HasForeignKey(e => e.SessionTypeId);

        builder.HasMany(e => e.Sessions)
                .WithOne(e => e.SessionType)
                .HasForeignKey(e => e.SessionTypeId);
    }
}