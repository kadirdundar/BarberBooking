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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure relationships and constraints here
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
        }
    }
}