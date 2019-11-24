﻿// <auto-generated />
using Client.Data.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Client.Data.Configuration.Migrations
{
    [DbContext(typeof(ScanServiceContext))]
    partial class ScanServiceContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.6-servicing-10079");

            modelBuilder.Entity("Client.Data.Entities.Resource", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Hash")
                        .IsRequired()
                        .HasMaxLength(64);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnName("name")
                        .HasMaxLength(256);

                    b.Property<string>("Path")
                        .IsRequired()
                        .HasColumnName("path")
                        .HasMaxLength(1024);

                    b.Property<long>("Size")
                        .HasColumnName("size");

                    b.HasKey("Id");

                    b.ToTable("resource");
                });
#pragma warning restore 612, 618
        }
    }
}