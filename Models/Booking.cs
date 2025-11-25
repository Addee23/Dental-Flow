namespace DentalFlow.Models
{
    public class Booking
    {
        public int Id { get; set; }
        public string? UserId { get; set; }
        public int ServiceId { get; set; }
        public Service? Service { get; set; }

        public DateTime DateTime { get; set; }
        public DateTime CreatedAt { get; set; }

        public string Name { get; set; } = "";
        public string Email { get; set; } = "";
        public string? Phone { get; set; }
    }
}
