﻿// <auto-generated />
using System;
using CuttingSystem3mkMobile.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace CuttingSystem3mkMobile.Models.Migrations
{
    [DbContext(typeof(Context))]
    [Migration("20190404205255_InitMainDataModel")]
    partial class InitMainDataModel
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.0-rtm-30799")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("CuttingSystem3mkMobile.Models.Models.CutCode", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Barcode");

                    b.Property<int>("IdCutModel");

                    b.Property<bool>("IsActive");

                    b.HasKey("Id");

                    b.HasIndex("IdCutModel");

                    b.ToTable("CutCode");
                });

            modelBuilder.Entity("CuttingSystem3mkMobile.Models.Models.CutFile", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<byte[]>("Value");

                    b.HasKey("Id");

                    b.ToTable("CutFile");
                });

            modelBuilder.Entity("CuttingSystem3mkMobile.Models.Models.CutModel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("IdCutFile");

                    b.Property<int>("IdDeviceModel");

                    b.Property<string>("Name");

                    b.Property<string>("QRCode");

                    b.HasKey("Id");

                    b.HasIndex("IdCutFile");

                    b.HasIndex("IdDeviceModel");

                    b.ToTable("CutModel");
                });

            modelBuilder.Entity("CuttingSystem3mkMobile.Models.Models.DeviceModel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.ToTable("DeviceModel");
                });

            modelBuilder.Entity("CuttingSystem3mkMobile.Models.Models.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Login");

                    b.Property<string>("Password");

                    b.HasKey("Id");

                    b.ToTable("User");
                });

            modelBuilder.Entity("CuttingSystem3mkMobile.Models.Models.CutCode", b =>
                {
                    b.HasOne("CuttingSystem3mkMobile.Models.Models.CutModel", "CutModel")
                        .WithMany("CutCodes")
                        .HasForeignKey("IdCutModel")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("CuttingSystem3mkMobile.Models.Models.CutModel", b =>
                {
                    b.HasOne("CuttingSystem3mkMobile.Models.Models.CutFile", "CutFile")
                        .WithMany()
                        .HasForeignKey("IdCutFile")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("CuttingSystem3mkMobile.Models.Models.DeviceModel", "DeviceModel")
                        .WithMany("CutModels")
                        .HasForeignKey("IdDeviceModel")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
