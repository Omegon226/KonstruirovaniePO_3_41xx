using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace DAL.EFClasses
{
    public partial class AvtovokzalDBContext : DbContext
    {
        public AvtovokzalDBContext()
            : base("name=AvtovokzalDBContext")
        {
        }

        public virtual DbSet<Cruise> Cruise { get; set; }
        public virtual DbSet<DayOfTheWeek> DayOfTheWeek { get; set; }
        public virtual DbSet<Driver> Driver { get; set; }
        public virtual DbSet<Locality> Locality { get; set; }
        public virtual DbSet<Route> Route { get; set; }
        public virtual DbSet<StoppingOnTheRoute> StoppingOnTheRoute { get; set; }
        public virtual DbSet<StopSequences> StopSequences { get; set; }
        public virtual DbSet<Ticket> Ticket { get; set; }
        public virtual DbSet<Transport> Transport { get; set; }
        public virtual DbSet<User> User { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Cruise>()
                .Property(e => e.StartTime)
                .HasPrecision(0);

            modelBuilder.Entity<Cruise>()
                .HasMany(e => e.Ticket)
                .WithRequired(e => e.Cruise)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<DayOfTheWeek>()
                .HasMany(e => e.Cruise)
                .WithRequired(e => e.DayOfTheWeek)
                .HasForeignKey(e => e.DayOfTheWeekCruiseID)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Driver>()
                .HasMany(e => e.Cruise)
                .WithRequired(e => e.Driver)
                .HasForeignKey(e => e.DriverIDOfTheCruise)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Locality>()
                .HasMany(e => e.StoppingOnTheRoute)
                .WithRequired(e => e.Locality)
                .HasForeignKey(e => e.StopLocalityID)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Route>()
                .HasMany(e => e.Cruise)
                .WithRequired(e => e.Route)
                .HasForeignKey(e => e.RouteIDOfTheCruise)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Route>()
                .HasMany(e => e.StopSequences)
                .WithRequired(e => e.Route)
                .HasForeignKey(e => e.StopRouteID)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<StoppingOnTheRoute>()
                .HasMany(e => e.StopSequences)
                .WithRequired(e => e.StoppingOnTheRoute)
                .HasForeignKey(e => e.StoppingID)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<StopSequences>()
                .Property(e => e.TravelTimeToStop)
                .HasPrecision(0);

            modelBuilder.Entity<Transport>()
                .HasMany(e => e.Cruise)
                .WithRequired(e => e.Transport)
                .HasForeignKey(e => e.TransportIDOfTheCruise)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<User>()
                .HasMany(e => e.Ticket)
                .WithRequired(e => e.User)
                .WillCascadeOnDelete(false);
        }
    }
}
