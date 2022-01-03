﻿// <auto-generated />
using System;
using JWT_Auth_ThirdExample.Auth;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace JWT_Auth_ThirdExample.Migrations
{
    [DbContext(typeof(UserContext))]
    [Migration("20210622093647_initial")]
    partial class initial
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.7")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("JWT_Auth_ThirdExample.Auth.LoginHistory", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("AccessTokenExpiryTime")
                        .HasColumnType("datetime2");

                    b.Property<bool>("AccessToken_Revoke")
                        .HasColumnType("bit");

                    b.Property<string>("Access_Token")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<DateTime>("LoginDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("MacAdress")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("ModifyLogin")
                        .HasColumnType("datetime2");

                    b.Property<string>("RefreshToken")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("RefreshTokenExpiryTime")
                        .HasColumnType("datetime2");

                    b.Property<bool>("RefreshToken_Revoke")
                        .HasColumnType("bit");

                    b.Property<long>("UserID")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("UserID");

                    b.ToTable("loginHistories");
                });

            modelBuilder.Entity("JWT_Auth_ThirdExample.Auth.Role", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("RoleName")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Roles");

                    b.HasData(
                        new
                        {
                            Id = 1L,
                            RoleName = "Manager"
                        });
                });

            modelBuilder.Entity("JWT_Auth_ThirdExample.Auth.User", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Password")
                        .HasColumnType("nvarchar(max)");

                    b.Property<long>("RoleId")
                        .HasColumnType("bigint");

                    b.Property<string>("UserName")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("Users");

                    b.HasData(
                        new
                        {
                            Id = 1L,
                            Password = "def@123",
                            RoleId = 1L,
                            UserName = "johndoe"
                        });
                });

            modelBuilder.Entity("JWT_Auth_ThirdExample.Auth.LoginHistory", b =>
                {
                    b.HasOne("JWT_Auth_ThirdExample.Auth.User", "User")
                        .WithMany()
                        .HasForeignKey("UserID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("JWT_Auth_ThirdExample.Auth.User", b =>
                {
                    b.HasOne("JWT_Auth_ThirdExample.Auth.Role", "Role")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Role");
                });
#pragma warning restore 612, 618
        }
    }
}
