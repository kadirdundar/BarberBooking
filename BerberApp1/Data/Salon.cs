using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace BerberApp1.Data
{
    public class Salon
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        [MaxLength(500)]
        public string? BannerUrl { get; set; }

        [MaxLength(200)]
        public string Address { get; set; } = string.Empty;

        [Phone]
        [MaxLength(20)]
        public string PhoneNumber { get; set; } = string.Empty;

        [EmailAddress]
        [MaxLength(100)]
        public string Email { get; set; } = string.Empty;

        [MaxLength(250)]
        public string Description { get; set; } = string.Empty;

        public int UserId { get; set; }
        public User? User { get; set; }

        public ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();
        public ICollection<SalonWorkingHours> WorkingHours { get; set; } = new List<SalonWorkingHours>();
        public ICollection<Service> Services { get; set; } = new List<Service>();
        public ICollection<Employee> Employees { get; set; } = new List<Employee>();
        public ICollection<SalonImage> Images { get; set; } = new List<SalonImage>();
    }
}