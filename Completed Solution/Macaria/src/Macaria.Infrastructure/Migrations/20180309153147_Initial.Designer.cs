﻿// <auto-generated />
using System;
using Macaria.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.Internal;

namespace Macaria.Infrastructure.Migrations
{
    [DbContext(typeof(MacariaContext))]
    [Migration("20180309153147_Initial")]
    partial class Initial
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.0-preview1-28290")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Macaria.Core.Entities.Note", b =>
                {
                    b.Property<int>("NoteId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Body");

                    b.Property<string>("CreatedBy");

                    b.Property<DateTime>("CreatedOn");

                    b.Property<bool>("IsDeleted");

                    b.Property<string>("LastModifiedBy");

                    b.Property<DateTime>("LastModifiedOn");

                    b.Property<string>("Slug");

                    b.Property<Guid?>("TenantId");

                    b.Property<string>("Title");

                    b.Property<int>("Version");

                    b.HasKey("NoteId");

                    b.HasIndex("TenantId");

                    b.ToTable("Notes");
                });

            modelBuilder.Entity("Macaria.Core.Entities.NoteTag", b =>
                {
                    b.Property<int>("NoteTagId")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("NoteId");

                    b.Property<int>("TagId");

                    b.HasKey("NoteTagId");

                    b.HasIndex("NoteId");

                    b.HasIndex("TagId");

                    b.ToTable("NoteTag");
                });

            modelBuilder.Entity("Macaria.Core.Entities.Tag", b =>
                {
                    b.Property<int>("TagId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("CreatedBy");

                    b.Property<DateTime>("CreatedOn");

                    b.Property<bool>("IsDeleted");

                    b.Property<string>("LastModifiedBy");

                    b.Property<DateTime>("LastModifiedOn");

                    b.Property<string>("Name");

                    b.Property<Guid?>("TenantId");

                    b.HasKey("TagId");

                    b.HasIndex("TenantId");

                    b.ToTable("Tags");
                });

            modelBuilder.Entity("Macaria.Core.Entities.Tenant", b =>
                {
                    b.Property<Guid>("TenantId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("CreatedBy");

                    b.Property<DateTime>("CreatedOn");

                    b.Property<bool>("IsDeleted");

                    b.Property<string>("LastModifiedBy");

                    b.Property<DateTime>("LastModifiedOn");

                    b.Property<string>("Name");

                    b.HasKey("TenantId");

                    b.ToTable("Tenants");
                });

            modelBuilder.Entity("Macaria.Core.Entities.User", b =>
                {
                    b.Property<int>("UserId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("CreatedBy");

                    b.Property<DateTime>("CreatedOn");

                    b.Property<bool>("IsDeleted");

                    b.Property<string>("LastModifiedBy");

                    b.Property<DateTime>("LastModifiedOn");

                    b.Property<Guid?>("TenantId");

                    b.HasKey("UserId");

                    b.HasIndex("TenantId");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("Macaria.Core.Entities.Note", b =>
                {
                    b.HasOne("Macaria.Core.Entities.Tenant", "Tenant")
                        .WithMany()
                        .HasForeignKey("TenantId");
                });

            modelBuilder.Entity("Macaria.Core.Entities.NoteTag", b =>
                {
                    b.HasOne("Macaria.Core.Entities.Note", "Note")
                        .WithMany("NoteTags")
                        .HasForeignKey("NoteId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Macaria.Core.Entities.Tag", "Tag")
                        .WithMany("NoteTags")
                        .HasForeignKey("TagId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Macaria.Core.Entities.Tag", b =>
                {
                    b.HasOne("Macaria.Core.Entities.Tenant", "Tenant")
                        .WithMany()
                        .HasForeignKey("TenantId");
                });

            modelBuilder.Entity("Macaria.Core.Entities.User", b =>
                {
                    b.HasOne("Macaria.Core.Entities.Tenant", "Tenant")
                        .WithMany()
                        .HasForeignKey("TenantId");
                });
#pragma warning restore 612, 618
        }
    }
}
