﻿// <auto-generated />
using System;
using MVC_EF_Start.DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace CollegeList.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20210717042540_CreateCollegeListDb")]
    partial class CreateCollegeListDb
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.8")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("CollegeList.Models.FieldOfStudy", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Degree")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid?>("InstitutionId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("InstitutionId");

                    b.ToTable("FieldOfStudy");
                });

            modelBuilder.Entity("CollegeList.Models.Institution", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("SchoolCity")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("SchoolName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("SchoolSchool_url")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("SchoolState")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Institution");
                });

            modelBuilder.Entity("CollegeList.Models.FieldOfStudy", b =>
                {
                    b.HasOne("CollegeList.Models.Institution", null)
                        .WithMany("FieldOfStudies")
                        .HasForeignKey("InstitutionId");
                });

            modelBuilder.Entity("CollegeList.Models.Institution", b =>
                {
                    b.Navigation("FieldOfStudies");
                });
#pragma warning restore 612, 618
        }
    }
}
