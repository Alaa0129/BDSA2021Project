﻿// <auto-generated />
using System;
using BlazorApp.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace BlazorApp.Infrastructure.Migrations
{
    [DbContext(typeof(PBankContext))]
    [Migration("20211204181936_2e0e5b68-82c4-415f-82ad-b5007f7a47ac")]
    partial class _2e0e5b6882c4415f82adb5007f7a47ac
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.11")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("BlazorApp.Infrastructure.Project", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("SupervisorId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("Id");

                    b.HasIndex("SupervisorId");

                    b.ToTable("Projects");
                });

            modelBuilder.Entity("BlazorApp.Infrastructure.Request", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(4400)
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("StudentId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("SupervisorId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("Id");

                    b.HasIndex("StudentId");

                    b.HasIndex("SupervisorId");

                    b.ToTable("Requests");
                });

            modelBuilder.Entity("BlazorApp.Infrastructure.Student", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<int?>("ProjectId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ProjectId");

                    b.ToTable("Students");
                });

            modelBuilder.Entity("BlazorApp.Infrastructure.Supervisor", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("Id");

                    b.ToTable("Supervisors");
                });

            modelBuilder.Entity("BlazorApp.Infrastructure.Tag", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("Id");

                    b.ToTable("Tags");
                });

            modelBuilder.Entity("ProjectTag", b =>
                {
                    b.Property<int>("ProjectsId")
                        .HasColumnType("int");

                    b.Property<int>("TagsId")
                        .HasColumnType("int");

                    b.HasKey("ProjectsId", "TagsId");

                    b.HasIndex("TagsId");

                    b.ToTable("ProjectTag");
                });

            modelBuilder.Entity("BlazorApp.Infrastructure.Project", b =>
                {
                    b.HasOne("BlazorApp.Infrastructure.Supervisor", "Supervisor")
                        .WithMany("Projects")
                        .HasForeignKey("SupervisorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Supervisor");
                });

            modelBuilder.Entity("BlazorApp.Infrastructure.Request", b =>
                {
                    b.HasOne("BlazorApp.Infrastructure.Student", "Student")
                        .WithMany("Requests")
                        .HasForeignKey("StudentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("BlazorApp.Infrastructure.Supervisor", "Supervisor")
                        .WithMany("Requests")
                        .HasForeignKey("SupervisorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Student");

                    b.Navigation("Supervisor");
                });

            modelBuilder.Entity("BlazorApp.Infrastructure.Student", b =>
                {
                    b.HasOne("BlazorApp.Infrastructure.Project", "Project")
                        .WithMany("AppliedStudents")
                        .HasForeignKey("ProjectId");

                    b.Navigation("Project");
                });

            modelBuilder.Entity("ProjectTag", b =>
                {
                    b.HasOne("BlazorApp.Infrastructure.Project", null)
                        .WithMany()
                        .HasForeignKey("ProjectsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("BlazorApp.Infrastructure.Tag", null)
                        .WithMany()
                        .HasForeignKey("TagsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("BlazorApp.Infrastructure.Project", b =>
                {
                    b.Navigation("AppliedStudents");
                });

            modelBuilder.Entity("BlazorApp.Infrastructure.Student", b =>
                {
                    b.Navigation("Requests");
                });

            modelBuilder.Entity("BlazorApp.Infrastructure.Supervisor", b =>
                {
                    b.Navigation("Projects");

                    b.Navigation("Requests");
                });
#pragma warning restore 612, 618
        }
    }
}