using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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
        public string? LocationId { get; set; }
        public int? TotalTickets { get; set; }
        public int TicketsSold { get; set; }

        [ForeignKey(nameof (BookingStatus))]
        public string? StatusId { get; set; }
        public StatusEntity? BookingStatus { get; set; }

        [ForeignKey(nameof(Category))]
        public string? CategoryId { get; set; }
        public CategoryEntity? Category { get; set; }
    }
}
