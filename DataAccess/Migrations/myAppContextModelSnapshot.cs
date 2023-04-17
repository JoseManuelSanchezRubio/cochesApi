﻿// <auto-generated />
using System;
using DataAccess.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace cochesApi.Migrations
{
    [DbContext(typeof(myAppContext))]
    partial class myAppContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.4")
                .HasAnnotation("Proxies:ChangeTracking", false)
                .HasAnnotation("Proxies:CheckEquality", false)
                .HasAnnotation("Proxies:LazyLoading", true);

            modelBuilder.Entity("cochesApi.Logic.Models.Branch", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Location")
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Branches");
                });

            modelBuilder.Entity("cochesApi.Logic.Models.Car", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("BranchId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Brand")
                        .HasColumnType("TEXT");

                    b.Property<string>("Model")
                        .HasColumnType("TEXT");

                    b.Property<int>("TypeCarId")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("isAutomatic")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("isGasoline")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("BranchId");

                    b.HasIndex("TypeCarId");

                    b.ToTable("Cars");
                });

            modelBuilder.Entity("cochesApi.Logic.Models.Customer", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Email")
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .HasColumnType("TEXT");

                    b.Property<string>("Password")
                        .HasColumnType("TEXT");

                    b.Property<string>("Surname")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Customers");
                });

            modelBuilder.Entity("cochesApi.Logic.Models.Planning", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("AvailableCars")
                        .HasColumnType("INTEGER");

                    b.Property<int>("BranchId")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("Day")
                        .HasColumnType("TEXT");

                    b.Property<int>("TypeCarId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.ToTable("Plannings");
                });

            modelBuilder.Entity("cochesApi.Logic.Models.Reservation", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("BranchId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("CarId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("CustomerId")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime?>("FinalDate")
                        .HasColumnType("TEXT");

                    b.Property<DateTime?>("InitialDate")
                        .HasColumnType("TEXT");

                    b.Property<int>("ReturnBranchId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("BranchId");

                    b.HasIndex("CarId");

                    b.HasIndex("CustomerId");

                    b.ToTable("Reservations");
                });

            modelBuilder.Entity("cochesApi.Logic.Models.TypeCar", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("TypeCars");
                });

            modelBuilder.Entity("cochesApi.Logic.Models.Car", b =>
                {
                    b.HasOne("cochesApi.Logic.Models.Branch", "Branch")
                        .WithMany("Cars")
                        .HasForeignKey("BranchId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("cochesApi.Logic.Models.TypeCar", "TypeCar")
                        .WithMany("Cars")
                        .HasForeignKey("TypeCarId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Branch");

                    b.Navigation("TypeCar");
                });

            modelBuilder.Entity("cochesApi.Logic.Models.Reservation", b =>
                {
                    b.HasOne("cochesApi.Logic.Models.Branch", "Branch")
                        .WithMany("Reservations")
                        .HasForeignKey("BranchId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("cochesApi.Logic.Models.Car", "Car")
                        .WithMany("Reservations")
                        .HasForeignKey("CarId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("cochesApi.Logic.Models.Customer", "Customer")
                        .WithMany("Reservations")
                        .HasForeignKey("CustomerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Branch");

                    b.Navigation("Car");

                    b.Navigation("Customer");
                });

            modelBuilder.Entity("cochesApi.Logic.Models.Branch", b =>
                {
                    b.Navigation("Cars");

                    b.Navigation("Reservations");
                });

            modelBuilder.Entity("cochesApi.Logic.Models.Car", b =>
                {
                    b.Navigation("Reservations");
                });

            modelBuilder.Entity("cochesApi.Logic.Models.Customer", b =>
                {
                    b.Navigation("Reservations");
                });

            modelBuilder.Entity("cochesApi.Logic.Models.TypeCar", b =>
                {
                    b.Navigation("Cars");
                });
#pragma warning restore 612, 618
        }
    }
}
