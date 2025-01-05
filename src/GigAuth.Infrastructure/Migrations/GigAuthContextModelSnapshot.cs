﻿// <auto-generated />
using System;
using GigAuth.Infrastructure.DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace GigAuth.Infrastructure.Migrations
{
    [DbContext(typeof(GigAuthContext))]
    partial class GigAuthContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("GigAuth.Domain.Entities.ForgotPasswordToken", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp with time zone")
                        .HasDefaultValueSql("CURRENT_TIMESTAMP");

                    b.Property<DateTime>("ExpirationDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Token")
                        .IsRequired()
                        .HasMaxLength(512)
                        .HasColumnType("character varying(512)");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("UserId")
                        .IsUnique();

                    b.HasIndex("UserId", "Token")
                        .IsUnique();

                    b.ToTable("ForgotPasswordTokens");
                });

            modelBuilder.Entity("GigAuth.Domain.Entities.Permission", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("CreatedDate")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp with time zone")
                        .HasDefaultValueSql("CURRENT_TIMESTAMP");

                    b.Property<string>("Description")
                        .HasMaxLength(500)
                        .HasColumnType("character varying(500)");

                    b.Property<bool>("IsActive")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("boolean")
                        .HasDefaultValue(true);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.Property<DateTime?>("UpdatedDate")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp with time zone")
                        .HasDefaultValueSql("CURRENT_TIMESTAMP");

                    b.HasKey("Id");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("Permissions");

                    b.HasData(
                        new
                        {
                            Id = new Guid("ba3b36b0-68c6-4bc7-84fd-2fac867ea86c"),
                            CreatedDate = new DateTime(2025, 1, 1, 12, 12, 59, 0, DateTimeKind.Utc),
                            IsActive = false,
                            Name = "User",
                            UpdatedDate = new DateTime(2025, 1, 1, 12, 12, 59, 0, DateTimeKind.Utc)
                        },
                        new
                        {
                            Id = new Guid("f55923e4-bcc8-4397-a9e3-2f9ff0bd025e"),
                            CreatedDate = new DateTime(2025, 1, 1, 12, 12, 59, 0, DateTimeKind.Utc),
                            IsActive = false,
                            Name = "Role",
                            UpdatedDate = new DateTime(2025, 1, 1, 12, 12, 59, 0, DateTimeKind.Utc)
                        },
                        new
                        {
                            Id = new Guid("f574d33c-d8bf-4dec-9173-09b6580f25ab"),
                            CreatedDate = new DateTime(2025, 1, 1, 12, 12, 59, 0, DateTimeKind.Utc),
                            IsActive = false,
                            Name = "Admin",
                            UpdatedDate = new DateTime(2025, 1, 1, 12, 12, 59, 0, DateTimeKind.Utc)
                        });
                });

            modelBuilder.Entity("GigAuth.Domain.Entities.RefreshToken", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("CreatedDate")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp with time zone")
                        .HasDefaultValueSql("CURRENT_TIMESTAMP");

                    b.Property<DateTime>("ExpirationDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Token")
                        .IsRequired()
                        .HasMaxLength(1000)
                        .HasColumnType("character varying(1000)");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("UserId")
                        .IsUnique();

                    b.ToTable("RefreshTokens");
                });

            modelBuilder.Entity("GigAuth.Domain.Entities.Role", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("CreatedDate")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp with time zone")
                        .HasDefaultValueSql("CURRENT_TIMESTAMP");

                    b.Property<string>("Description")
                        .HasMaxLength(500)
                        .HasColumnType("character varying(500)");

                    b.Property<bool>("IsActive")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("boolean")
                        .HasDefaultValue(true);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.Property<DateTime?>("UpdatedDate")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp with time zone")
                        .HasDefaultValueSql("CURRENT_TIMESTAMP");

                    b.HasKey("Id");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("Roles");

                    b.HasData(
                        new
                        {
                            Id = new Guid("728e5486-ddd3-42cd-b8c5-3278181b1d36"),
                            CreatedDate = new DateTime(2025, 1, 1, 12, 12, 59, 0, DateTimeKind.Utc),
                            IsActive = false,
                            Name = "Admin",
                            UpdatedDate = new DateTime(2025, 1, 1, 12, 12, 59, 0, DateTimeKind.Utc)
                        },
                        new
                        {
                            Id = new Guid("9eaeca53-2cfc-409c-a411-63bf7f69f8c6"),
                            CreatedDate = new DateTime(2025, 1, 1, 12, 12, 59, 0, DateTimeKind.Utc),
                            IsActive = false,
                            Name = "Manager",
                            UpdatedDate = new DateTime(2025, 1, 1, 12, 12, 59, 0, DateTimeKind.Utc)
                        },
                        new
                        {
                            Id = new Guid("f66caaf2-f359-4aee-a057-784023736d67"),
                            CreatedDate = new DateTime(2025, 1, 1, 12, 12, 59, 0, DateTimeKind.Utc),
                            IsActive = false,
                            Name = "User",
                            UpdatedDate = new DateTime(2025, 1, 1, 12, 12, 59, 0, DateTimeKind.Utc)
                        });
                });

            modelBuilder.Entity("GigAuth.Domain.Entities.RolePermission", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uuid");

                    b.Property<Guid>("PermissionId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("RoleId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("PermissionId");

                    b.HasIndex("RoleId");

                    b.ToTable("RolePermission");

                    b.HasData(
                        new
                        {
                            Id = new Guid("97773159-aa53-4761-8c27-d87705dd9280"),
                            PermissionId = new Guid("ba3b36b0-68c6-4bc7-84fd-2fac867ea86c"),
                            RoleId = new Guid("9eaeca53-2cfc-409c-a411-63bf7f69f8c6")
                        },
                        new
                        {
                            Id = new Guid("3f4760f9-f709-41b1-a07c-1d4b914f53f3"),
                            PermissionId = new Guid("f55923e4-bcc8-4397-a9e3-2f9ff0bd025e"),
                            RoleId = new Guid("9eaeca53-2cfc-409c-a411-63bf7f69f8c6")
                        },
                        new
                        {
                            Id = new Guid("f9210a4e-fdaf-4cb2-a1b0-18925b493d6a"),
                            PermissionId = new Guid("f574d33c-d8bf-4dec-9173-09b6580f25ab"),
                            RoleId = new Guid("728e5486-ddd3-42cd-b8c5-3278181b1d36")
                        });
                });

            modelBuilder.Entity("GigAuth.Domain.Entities.User", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("CreatedDate")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp with time zone")
                        .HasDefaultValueSql("CURRENT_TIMESTAMP");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<bool>("IsActive")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("boolean")
                        .HasDefaultValue(true);

                    b.Property<string>("PasswordHash")
                        .IsRequired()
                        .HasMaxLength(512)
                        .HasColumnType("character varying(512)");

                    b.Property<DateTime?>("UpdatedDate")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp with time zone")
                        .HasDefaultValueSql("CURRENT_TIMESTAMP");

                    b.Property<string>("UserName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.HasKey("Id");

                    b.HasIndex("Email")
                        .IsUnique();

                    b.HasIndex("UserName")
                        .IsUnique();

                    b.ToTable("Users");
                });

            modelBuilder.Entity("GigAuth.Domain.Entities.UserRole", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uuid");

                    b.Property<Guid>("RoleId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.HasIndex("UserId");

                    b.ToTable("UserRole");
                });

            modelBuilder.Entity("GigAuth.Domain.Entities.ForgotPasswordToken", b =>
                {
                    b.HasOne("GigAuth.Domain.Entities.User", null)
                        .WithOne()
                        .HasForeignKey("GigAuth.Domain.Entities.ForgotPasswordToken", "UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("GigAuth.Domain.Entities.RefreshToken", b =>
                {
                    b.HasOne("GigAuth.Domain.Entities.User", "User")
                        .WithOne()
                        .HasForeignKey("GigAuth.Domain.Entities.RefreshToken", "UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("GigAuth.Domain.Entities.RolePermission", b =>
                {
                    b.HasOne("GigAuth.Domain.Entities.Permission", "Permission")
                        .WithMany("RolePermissions")
                        .HasForeignKey("PermissionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("GigAuth.Domain.Entities.Role", "Role")
                        .WithMany("RolePermissions")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Permission");

                    b.Navigation("Role");
                });

            modelBuilder.Entity("GigAuth.Domain.Entities.UserRole", b =>
                {
                    b.HasOne("GigAuth.Domain.Entities.Role", "Role")
                        .WithMany("UserRoles")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("GigAuth.Domain.Entities.User", "User")
                        .WithMany("UserRoles")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Role");

                    b.Navigation("User");
                });

            modelBuilder.Entity("GigAuth.Domain.Entities.Permission", b =>
                {
                    b.Navigation("RolePermissions");
                });

            modelBuilder.Entity("GigAuth.Domain.Entities.Role", b =>
                {
                    b.Navigation("RolePermissions");

                    b.Navigation("UserRoles");
                });

            modelBuilder.Entity("GigAuth.Domain.Entities.User", b =>
                {
                    b.Navigation("UserRoles");
                });
#pragma warning restore 612, 618
        }
    }
}
