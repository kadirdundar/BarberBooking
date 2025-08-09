using Microsoft.EntityFrameworkCore;

namespace BerberApp1.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        // Add DbSet properties for your new entities
        public DbSet<Salon> Salons { get; set; }
        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<SalonWorkingHours> SalonWorkingHours { get; set; }
        public DbSet<Service> Services { get; set; }
        public DbSet<AppointmentService> AppointmentServices { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Employee> Employees { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure relationships and constraints here
            modelBuilder.Entity<AppointmentService>()
                .HasKey(asl => new { asl.AppointmentId, asl.ServiceId });

            modelBuilder.Entity<AppointmentService>()
                .HasOne(asl => asl.Appointment)
                .WithMany(a => a.AppointmentServices)
                .HasForeignKey(asl => asl.AppointmentId);

            modelBuilder.Entity<AppointmentService>()
                .HasOne(asl => asl.Service)
                .WithMany(s => s.AppointmentServices)
                .HasForeignKey(asl => asl.ServiceId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Appointment>()
                .HasOne(a => a.Salon)
                .WithMany(s => s.Appointments)
                .HasForeignKey(a => a.SalonId);

            modelBuilder.Entity<SalonWorkingHours>()
                .HasOne(swh => swh.Salon)
                .WithMany(s => s.WorkingHours)
                .HasForeignKey(swh => swh.SalonId);
            
            modelBuilder.Entity<Service>()
                .HasOne(s => s.Salon)
                .WithMany(s => s.Services)
                .HasForeignKey(s => s.SalonId);

            modelBuilder.Entity<Salon>()
                .HasOne(s => s.User)
                .WithMany(u => u.Salons)
                .HasForeignKey(s => s.UserId);

            modelBuilder.Entity<Employee>()
                .HasOne(e => e.Salon)
                .WithMany(s => s.Employees)
                .HasForeignKey(e => e.SalonId);

            modelBuilder.Entity<Appointment>()
                .HasOne(a => a.Employee)
                .WithMany(e => e.Appointments)
                .HasForeignKey(a => a.EmployeeId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}