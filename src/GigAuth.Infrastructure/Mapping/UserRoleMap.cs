using GigAuth.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GigAuth.Infrastructure.Mapping;

public class UserRoleMap : IEntityTypeConfiguration<UserRole>
{
    public void Configure(EntityTypeBuilder<UserRole> builder)
    {
        builder.HasKey(rp => rp.Id);
        builder.Property(r => r.Id)
            .IsRequired()
            .ValueGeneratedNever();

        builder.HasOne(rp => rp.User)
            .WithMany(r => r.UserRoles)
            .HasForeignKey(rp => rp.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(rp => rp.Role)
            .WithMany(p => p.UserRoles)
            .HasForeignKey(rp => rp.RoleId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}