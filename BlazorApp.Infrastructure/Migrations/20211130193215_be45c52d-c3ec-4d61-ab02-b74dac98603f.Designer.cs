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
    [Migration("20211130193215_be45c52d-c3ec-4d61-ab02-b74dac98603f")]
    partial class be45c52dc3ec4d61ab02b74dac98603f
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
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<int>("MaxApplications")
                        .HasColumnType("int");

                    b.Property<int>("SupervisorId")
                        .HasColumnType("int");

                    b.Property<string>("Title")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("Id");

                    b.ToTable("Projects");
                });

            modelBuilder.Entity("BlazorApp.Infrastructure.Request", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Description")
                        .HasMaxLength(1000)
                        .HasColumnType("nvarchar(1000)");

                    b.Property<int>("StudentId")
                        .HasColumnType("int");

                    b.Property<string>("Title")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("Id");

                    b.ToTable("Requests");
                });

            modelBuilder.Entity("BlazorApp.Infrastructure.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Firstname")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Lastname")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<int?>("ProjectId")
                        .HasColumnType("int");

                    b.Property<int?>("RequestId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ProjectId");

                    b.HasIndex("RequestId");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("BlazorApp.Infrastructure.User", b =>
                {
                    b.HasOne("BlazorApp.Infrastructure.Project", null)
                        .WithMany("AppliedStudents")
                        .HasForeignKey("ProjectId");

                    b.HasOne("BlazorApp.Infrastructure.Request", null)
                        .WithMany("AppliedStudents")
                        .HasForeignKey("RequestId");
                });

            modelBuilder.Entity("BlazorApp.Infrastructure.Project", b =>
                {
                    b.Navigation("AppliedStudents");
                });

            modelBuilder.Entity("BlazorApp.Infrastructure.Request", b =>
                {
                    b.Navigation("AppliedStudents");
                });
#pragma warning restore 612, 618
        }
    }
}