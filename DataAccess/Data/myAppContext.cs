using Microsoft.EntityFrameworkCore;
using cochesApi.Logic.Models;
using System.Data;
using Microsoft.Data.Sqlite;

namespace DataAccess.Data
{
    public class myAppContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=DataAccess/cochesApi.db");
        }
        public myAppContext(DbContextOptions<myAppContext> options) : base(options)
        {

        }
        public myAppContext(){
            
        }


        public DbSet<Branch> Branches { get; set; } = null!;
        public DbSet<Customer> Customers { get; set; } = null!;
        public DbSet<Car> Cars { get; set; } = null!;
        public DbSet<Reservation> Reservations { get; set; } = null!;
        public DbSet<TypeCar> TypeCars { get; set; } = null!;
        public DbSet<Planning> Plannings { get; set; } = default!;


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Branch>()
                .HasMany(e => e.Cars)
                .WithOne(e => e.Branch)
                .HasForeignKey(e => e.BranchId)
                .IsRequired();
            modelBuilder.Entity<Branch>()
                .HasMany(e => e.Reservations)
                .WithOne(e => e.Branch)
                .HasForeignKey(e => e.BranchId)
                .IsRequired();



            modelBuilder.Entity<Car>()
                .HasMany(e => e.Reservations)
                .WithOne(e => e.Car)
                .HasForeignKey(e => e.CarId)
                .IsRequired();
            modelBuilder.Entity<Car>()
                .HasOne(e => e.Branch)
                .WithMany(e => e.Cars)
                .HasForeignKey(e => e.BranchId)
                .IsRequired();
            modelBuilder.Entity<Car>()
                .HasOne(e => e.TypeCar)
                .WithMany(e => e.Cars)
                .HasForeignKey(e => e.TypeCarId)
                .IsRequired();



            modelBuilder.Entity<Customer>()
                .HasMany(e => e.Reservations)
                .WithOne(e => e.Customer)
                .HasForeignKey(e => e.CustomerId)
                .IsRequired();


            modelBuilder.Entity<Reservation>()
                .HasOne(e => e.Car)
                .WithMany(e => e.Reservations)
                .HasForeignKey(e => e.CarId)
                .IsRequired();
            modelBuilder.Entity<Reservation>()
                .HasOne(e => e.Customer)
                .WithMany(e => e.Reservations)
                .HasForeignKey(e => e.CustomerId)
                .IsRequired();
            modelBuilder.Entity<Reservation>()
                .HasOne(e => e.Branch)
                .WithMany(e => e.Reservations)
                .HasForeignKey(e => e.BranchId)
                .IsRequired();


            modelBuilder.Entity<TypeCar>()
                .HasMany(e => e.Cars)
                .WithOne(e => e.TypeCar)
                .HasForeignKey(e => e.TypeCarId)
                .IsRequired();

        }




    }
}