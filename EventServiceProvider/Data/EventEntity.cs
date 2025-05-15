using System.ComponentModel.DataAnnotations;

namespace EventServiceProvider.Data
{
    public class EventEntity
    {
        [Key]
        public string EventId { get; set; } = Guid.NewGuid().ToString();
        public string EventTitle { get; set; } = null!;
        public string Description { get; set; } = null!;
        public DateTime? Date { get; set; }
        public int? Price { get; set; }
        public string? BookingStatus { get; set; }
        public string? Category { get; set; }
        public string? LocationId { get; set; }
        public int? TotalTickets { get; set; }
        public int TicketsSold { get; set; }
    }
}
