using EventServiceProvider.Data;

namespace EventServiceProvider.Factories;

public class EventFactory
{
    public static EventEntity ToEntity(Event request)
    {
        if (request == null)
        {
            return null!;
        }
        return new EventEntity
        {
            EventId = request.EventId,
            EventTitle = request.EventTitle,
            Description = request.Description,
            Date = request.Date?.ToDateTime() ?? null,
            Price = request.Price,
            StatusId = request.Status?.StatusId,
            CategoryId = request.Category?.CategoryId,
            LocationId = request.LocationId,
            TotalTickets = request.TotalTickets,
            TicketsSold = request.TicketsSold
        };

    }


}
