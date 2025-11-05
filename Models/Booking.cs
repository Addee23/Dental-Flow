using System;

namespace DentalFlow.Models
{
    public class Booking
    {
        public int Id { get; set; }
        public string UserId { get; set; } = "";
        public int ServiceId { get; set; }
        public DateTime Date { get; set; }
        public TimeSpan Time { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
