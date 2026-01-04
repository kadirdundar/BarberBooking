using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BerberApp1.Data
{
    public class User
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(255)]
        public string? GoogleId { get; set; }

        [Required]
        [MaxLength(255)]
        public string? Email { get; set; }

        [Required]
        [MaxLength(255)]
        public string? Name { get; set; }
        
        public string? PictureUrl { get; set; }

        public ICollection<Salon> Salons { get; set; } = new List<Salon>();
    }
}