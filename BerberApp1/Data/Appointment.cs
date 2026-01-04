using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BerberApp1.Data
{
    public class Appointment
    {
        public int Id { get; set; }

        [Required]
        public int SalonId { get; set; }

        [Required]
        public DateTime AppointmentDate { get; set; }

        [Required]
        public TimeSpan AppointmentTime { get; set; }

        [Required]
        [MaxLength(100)]
        public string CustomerName { get; set; } = string.Empty;

        [Phone]
        [MaxLength(20)]
        public string CustomerPhone { get; set; } = string.Empty;

        [MaxLength(250)]
        public string Notes { get; set; } = string.Empty;

        public Salon? Salon { get; set; }
        public int EmployeeId { get; set; }
        public Employee? Employee { get; set; }
        public ICollection<AppointmentService> AppointmentServices { get; set; } = new List<AppointmentService>();
        public bool IsBreak { get; set; } = false;
        public int? DurationInMinutes { get; set; } // For breaks
    }
}