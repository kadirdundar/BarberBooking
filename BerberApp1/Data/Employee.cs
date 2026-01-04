using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BerberApp1.Data
{
    public class Employee
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        [MaxLength(255)]
        public string PictureUrl { get; set; } = string.Empty;

        [Required]
        public int SalonId { get; set; }
        public Salon? Salon { get; set; }

        public ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();
    }
}