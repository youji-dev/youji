﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using PersistenceLayer.DataAccess;

#nullable disable

namespace PersistenceLayer.DataAccess.Migrations
{
    [DbContext(typeof(DataContext))]
    [Migration("20240920040708_update")]
    partial class update
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.HasPostgresExtension(modelBuilder, "uuid-ossp");
            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("PersistenceLayer.DataAccess.Entities.Building", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasDefaultValueSql("uuid_generate_v4()");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Buildings");
                });

            modelBuilder.Entity("PersistenceLayer.DataAccess.Entities.Priority", b =>
                {
                    b.Property<int>("Value")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Value"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Value");

                    b.ToTable("Priorities");
                });

            modelBuilder.Entity("PersistenceLayer.DataAccess.Entities.RoleAssignment", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("text");

                    b.Property<int>("Type")
                        .HasColumnType("integer");

                    b.HasKey("UserId");

                    b.ToTable("RoleAssignments");
                });

            modelBuilder.Entity("PersistenceLayer.DataAccess.Entities.State", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasDefaultValueSql("uuid_generate_v4()");

                    b.Property<string>("Color")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("States");
                });

            modelBuilder.Entity("PersistenceLayer.DataAccess.Entities.Ticket", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasDefaultValueSql("uuid_generate_v4()");

                    b.Property<string>("Author")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<Guid?>("BuildingId")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("CreationDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Description")
                        .HasColumnType("text");

                    b.Property<string>("Object")
                        .HasColumnType("text");

                    b.Property<int?>("PriorityValue")
                        .HasColumnType("integer");

                    b.Property<string>("Room")
                        .HasColumnType("text");

                    b.Property<Guid>("StateId")
                        .HasColumnType("uuid");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("BuildingId");

                    b.HasIndex("PriorityValue");

                    b.HasIndex("StateId");

                    b.ToTable("Tickets");
                });

            modelBuilder.Entity("PersistenceLayer.DataAccess.Entities.TicketAttachment", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasDefaultValueSql("uuid_generate_v4()");

                    b.Property<byte[]>("Binary")
                        .IsRequired()
                        .HasColumnType("bytea");

                    b.Property<string>("FileType")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<Guid?>("TicketId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("TicketId");

                    b.ToTable("Attachments");
                });

            modelBuilder.Entity("PersistenceLayer.DataAccess.Entities.TicketComment", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasDefaultValueSql("uuid_generate_v4()");

                    b.Property<string>("Author")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("CreationDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid?>("TicketId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("TicketId");

                    b.ToTable("Comments");
                });

            modelBuilder.Entity("PersistenceLayer.DataAccess.Entities.Ticket", b =>
                {
                    b.HasOne("PersistenceLayer.DataAccess.Entities.Building", "Building")
                        .WithMany()
                        .HasForeignKey("BuildingId");

                    b.HasOne("PersistenceLayer.DataAccess.Entities.Priority", "Priority")
                        .WithMany()
                        .HasForeignKey("PriorityValue");

                    b.HasOne("PersistenceLayer.DataAccess.Entities.State", "State")
                        .WithMany()
                        .HasForeignKey("StateId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Building");

                    b.Navigation("Priority");

                    b.Navigation("State");
                });

            modelBuilder.Entity("PersistenceLayer.DataAccess.Entities.TicketAttachment", b =>
                {
                    b.HasOne("PersistenceLayer.DataAccess.Entities.Ticket", null)
                        .WithMany("Attachments")
                        .HasForeignKey("TicketId");
                });

            modelBuilder.Entity("PersistenceLayer.DataAccess.Entities.TicketComment", b =>
                {
                    b.HasOne("PersistenceLayer.DataAccess.Entities.Ticket", null)
                        .WithMany("Comments")
                        .HasForeignKey("TicketId");
                });

            modelBuilder.Entity("PersistenceLayer.DataAccess.Entities.Ticket", b =>
                {
                    b.Navigation("Attachments");

                    b.Navigation("Comments");
                });
#pragma warning restore 612, 618
        }
    }
}
