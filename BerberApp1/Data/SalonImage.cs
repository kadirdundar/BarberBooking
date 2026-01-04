namespace BerberApp1.Data
{
    public class SalonImage
    {
        public int Id { get; set; }
        public int SalonId { get; set; }
        public string ImageUrl { get; set; } = string.Empty;
        public Salon? Salon { get; set; }
    }
}