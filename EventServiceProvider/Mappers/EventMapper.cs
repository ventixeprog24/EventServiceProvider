using EventServiceProvider.Data;
using Google.Protobuf.WellKnownTypes;
using LocationServiceProvider;
using static LocationServiceProvider.LocationServiceContract;

namespace EventServiceProvider.Mappers;

public class EventMapper(LocationServiceContractClient locationServiceClient)
{
    private readonly LocationServiceContractClient _locationServiceClient = locationServiceClient;

    public async Task<Event> MapToGrpcEvent(EventEntity eventEntity)
    {
        var location = new Location();
        if (!string.IsNullOrEmpty(eventEntity.LocationId))
        {
            var reply = await _locationServiceClient.GetLocationByIdAsync(new LocationByIdRequest { Id = eventEntity.LocationId });
            if(reply.Succeeded && reply.Location != null)
            {
                location = reply.Location;
            }
            else
            {
                // Handle the case where the location is not found or the request failed
                location.Id = eventEntity.LocationId;
            }
        }
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
            },
            Location = location
        };
    }
}
