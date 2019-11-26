﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Service.Data.Configuration;

namespace Service.Data.Configuration.Migrations
{
    [DbContext(typeof(AvailabilityServiceContext))]
    [Migration("20191125220728_Initial")]
    partial class Initial
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.6-servicing-10079")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Service.Data.Entities.ClientAvailability", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ClientId")
                        .IsRequired()
                        .HasColumnName("client_id")
                        .HasMaxLength(256);

                    b.Property<string>("Message")
                        .IsRequired()
                        .HasColumnName("message")
                        .HasMaxLength(1024);

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasColumnName("status")
                        .HasMaxLength(50);

                    b.Property<DateTimeOffset>("TimeStamp")
                        .HasColumnName("timestamp")
                        .HasColumnType("datetimeoffset(3)");

                    b.HasKey("Id");

                    b.ToTable("client_availability");
                });
#pragma warning restore 612, 618
        }
    }
}
