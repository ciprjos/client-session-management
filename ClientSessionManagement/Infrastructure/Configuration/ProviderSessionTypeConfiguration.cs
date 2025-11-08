using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configuration;
internal sealed class ProviderSessionTypeConfiguration : IEntityTypeConfiguration<ProviderSessionType>
{
    public void Configure(EntityTypeBuilder<ProviderSessionType> builder)
    {
        builder.HasKey(e => new { e.ProviderId, e.SessionTypeId });

        builder.HasOne(e => e.Provider)
                .WithMany(e => e.ProviderSessionTypes)
                .HasForeignKey(e => e.ProviderId);

        builder.HasOne(e => e.SessionType)
                .WithMany(e => e.ProviderSessionTypes)
                .HasForeignKey(e => e.SessionTypeId);
    }
}