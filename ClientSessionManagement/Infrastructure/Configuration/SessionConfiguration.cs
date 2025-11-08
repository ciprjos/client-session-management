using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configuration;
internal sealed class SessionConfiguration : IEntityTypeConfiguration<Session>
{
    public void Configure(EntityTypeBuilder<Session> builder)
    {
        builder.HasKey(e => e.Id);

        builder.Property(e => e.SessionDate)
                .IsRequired();

        builder.Property(e => e.Notes)
                .IsRequired(false);

        builder.HasOne(e => e.Client)
                .WithMany(e => e.Sessions)
                .HasForeignKey(e => e.ClientId);

        builder.HasOne(e => e.Provider)
                .WithMany(e => e.Sessions)
                .HasForeignKey(e => e.ProviderId);

        builder.HasOne(e => e.SessionType)
                .WithMany(e => e.Sessions)
                .HasForeignKey(e => e.SessionTypeId);

        builder.Property(e => e.CreatedAt)
                .IsRequired();

        builder.Property(e => e.UpdatedAt)
                .IsRequired(false);
    }
}
