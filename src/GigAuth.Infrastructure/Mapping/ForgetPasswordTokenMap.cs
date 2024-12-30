using GigAuth.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GigAuth.Infrastructure.Mapping;

public class ForgetPasswordTokenMap : IEntityTypeConfiguration<ForgotPasswordToken>
{
    public void Configure(EntityTypeBuilder<ForgotPasswordToken> builder)
    {
        builder.HasKey(f => f.Id);

        builder.Property(f => f.Id)
            .IsRequired()
            .ValueGeneratedNever();

        builder.Property(f => f.Token)
            .IsRequired()
            .HasMaxLength(512);

        builder.Property(f => f.ExpirationDate)
            .IsRequired()
            .HasColumnType("timestamp with time zone");

        builder.Property(f => f.CreatedAt)
            .IsRequired()
            .HasColumnType("timestamp with time zone")
            .HasDefaultValueSql("CURRENT_TIMESTAMP");

        builder.HasIndex(f => new { f.UserId, f.Token })
            .IsUnique();

        builder.HasOne<User>()
            .WithOne()
            .HasForeignKey<ForgotPasswordToken>(f => f.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}