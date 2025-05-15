using EventServiceProvider.Data;
using Google.Protobuf.WellKnownTypes;

namespace EventServiceProvider.Mappers;

public class EventMapper
{
    public static Event MapToGrpcEvent(EventEntity eventEntity)
    {
        return new Event
        {
            EventId = eventEntity.EventId,
            EventTitle = eventEntity.EventTitle,
            Description = eventEntity.Description,
            Date = eventEntity.Date.HasValue ? Timestamp.FromDateTime(eventEntity.Date.Value.ToUniversalTime()) : null,
            Price = eventEntity.Price ?? 0,
            BookingStatus = eventEntity.BookingStatus,
            Category = eventEntity.Category,
            LocationId = eventEntity.LocationId,
            TotalTickets = eventEntity.TotalTickets ?? 0,
            TicketsSold = eventEntity.TicketsSold
        };
    }
}
