﻿// <auto-generated />
using System;
using Data.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Data.Migrations
{
    [DbContext(typeof(DataContext))]
    [Migration("20241007222607_addStatus")]
    partial class addStatus
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .UseIdentityColumns()
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.10");

            modelBuilder.Entity("Data.AuthEntities.Role", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Slug")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Role");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Name = "Administrador"
                        },
                        new
                        {
                            Id = 2,
                            Name = "Gestor"
                        },
                        new
                        {
                            Id = 3,
                            Name = "Publicador"
                        },
                        new
                        {
                            Id = 4,
                            Name = "Revisador"
                        },
                        new
                        {
                            Id = 5,
                            Name = "User"
                        });
                });

            modelBuilder.Entity("Data.AuthEntities.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FirstName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<bool>("IsEmailVerified")
                        .HasColumnType("bit");

                    b.Property<string>("LastName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Password")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ProfilePicture")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("RoleId")
                        .HasColumnType("int");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("Username")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("VerifyCode")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("VerifyCodeDate")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("Users");

                    b.HasData(
                        new
                        {
                            Id = new Guid("11111111-1111-1111-1111-111111111111"),
                            CreatedAt = new DateTime(2024, 10, 7, 22, 26, 6, 502, DateTimeKind.Utc).AddTicks(2002),
                            Email = "admin@admin.com",
                            FirstName = "Administrador de Sistema",
                            IsActive = true,
                            IsDeleted = false,
                            IsEmailVerified = false,
                            LastName = "Administrador de Sistema",
                            Password = "0f3d85258d593088098f65c26e89d49bf1bcf29b2b57dcfc36865ecefec7551fb8f232a028d98bd39acfb2710ef5e6e8f08e5a4ddbc213a82ad6008e64861abd",
                            RoleId = 1,
                            Username = "admin",
                            VerifyCodeDate = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified)
                        });
                });

            modelBuilder.Entity("Data.Entities.GeneralEntities.AppSettings", b =>
                {
                    b.Property<string>("Key")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Description")
                        .HasMaxLength(4000)
                        .HasColumnType("nvarchar(4000)");

                    b.Property<string>("Value")
                        .HasMaxLength(4000)
                        .HasColumnType("nvarchar(4000)");

                    b.HasKey("Key");

                    b.ToTable("AppConfigurations");

                    b.HasData(
                        new
                        {
                            Key = "WesenderAppKey",
                            Description = "Acesso Wesender",
                            Value = ""
                        });
                });

            modelBuilder.Entity("Data.NewsVerfication.News", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<string>("CoverUrl")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<bool>("IsPublished")
                        .HasColumnType("bit");

                    b.Property<int>("Like")
                        .HasColumnType("int");

                    b.Property<DateTime>("PublicationDate")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("PublishedById")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("ReadTime")
                        .HasColumnType("int");

                    b.Property<string>("Resume")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("TagId")
                        .HasColumnType("int");

                    b.Property<string>("Text")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("UnLike")
                        .HasColumnType("int");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("datetime2");

                    b.Property<int>("VerificationId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("PublishedById");

                    b.HasIndex("TagId");

                    b.HasIndex("VerificationId")
                        .IsUnique();

                    b.ToTable("News");
                });

            modelBuilder.Entity("Data.NewsVerfication.Tag", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.ToTable("Tags");
                });

            modelBuilder.Entity("Data.NewsVerfication.Verification", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<string>("Attachments")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<string>("MainLink")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Obs")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PublishedChannel")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("PublishedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("PublishedTitle")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("RequestedById")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("SecundaryLink")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("VerificationDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("VerificationStatusId")
                        .HasColumnType("int");

                    b.Property<Guid?>("VerifiedById")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("RequestedById");

                    b.HasIndex("VerificationStatusId");

                    b.HasIndex("VerifiedById");

                    b.ToTable("Verifications");
                });

            modelBuilder.Entity("Data.NewsVerfication.VerificationStatus", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("VerificationStatus");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Name = "Verdadeira"
                        },
                        new
                        {
                            Id = 2,
                            Name = "Falsa"
                        },
                        new
                        {
                            Id = 3,
                            Name = "Em Revisao"
                        },
                        new
                        {
                            Id = 4,
                            Name = "Pendente Revisão"
                        });
                });

            modelBuilder.Entity("Data.AuthEntities.User", b =>
                {
                    b.HasOne("Data.AuthEntities.Role", "Role")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Role");
                });

            modelBuilder.Entity("Data.NewsVerfication.News", b =>
                {
                    b.HasOne("Data.AuthEntities.User", "PublishedBy")
                        .WithMany()
                        .HasForeignKey("PublishedById")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("Data.NewsVerfication.Tag", "Tag")
                        .WithMany("NewsArticles")
                        .HasForeignKey("TagId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("Data.NewsVerfication.Verification", "Verification")
                        .WithOne("News")
                        .HasForeignKey("Data.NewsVerfication.News", "VerificationId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("PublishedBy");

                    b.Navigation("Tag");

                    b.Navigation("Verification");
                });

            modelBuilder.Entity("Data.NewsVerfication.Verification", b =>
                {
                    b.HasOne("Data.AuthEntities.User", "RequestedBy")
                        .WithMany()
                        .HasForeignKey("RequestedById")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("Data.NewsVerfication.VerificationStatus", "VerificationStatus")
                        .WithMany()
                        .HasForeignKey("VerificationStatusId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("Data.AuthEntities.User", "VerifiedBy")
                        .WithMany()
                        .HasForeignKey("VerifiedById");

                    b.Navigation("RequestedBy");

                    b.Navigation("VerificationStatus");

                    b.Navigation("VerifiedBy");
                });

            modelBuilder.Entity("Data.NewsVerfication.Tag", b =>
                {
                    b.Navigation("NewsArticles");
                });

            modelBuilder.Entity("Data.NewsVerfication.Verification", b =>
                {
                    b.Navigation("News");
                });
#pragma warning restore 612, 618
        }
    }
}
