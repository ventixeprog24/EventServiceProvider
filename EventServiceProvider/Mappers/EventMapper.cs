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
            Date = eventEntity.Date.HasValue ? Timestamp.FromDateTime(eventEntity.Date.Value.ToUniversalTime()) : new Timestamp(),
            Price = eventEntity.Price ?? 0,
            LocationId = eventEntity.LocationId ?? string.Empty,
            TotalTickets = eventEntity.TotalTickets ?? 0,
            TicketsSold = eventEntity.TicketsSold,
            Status = new Status
            {
                StatusId = eventEntity.BookingStatus?.StatusId,
                StatusName = eventEntity.BookingStatus?.StatusName,
            },
            Category = new Category
            {
                CategoryId = eventEntity.Category?.CategoryId ?? string.Empty,
                CategoryName = eventEntity.Category?.CategoryName ?? string.Empty,
            }
        };
    }
}
