using System;
using System.ComponentModel.DataAnnotations;

namespace BerberApp1.Data
{
    public class SalonWorkingHours
    {
        public int Id { get; set; }

        [Required]
        public int SalonId { get; set; }

        [Required]
        public DayOfWeek DayOfWeek { get; set; }

        [Required]
        public TimeOnly OpeningTime { get; set; }

        [Required]
        public TimeOnly ClosingTime { get; set; }

        public bool IsClosed { get; set; } = false;

        public Salon? Salon { get; set; }
    }
} 