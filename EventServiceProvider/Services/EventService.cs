using Azure.Core;
using EventServiceProvider.Factories;
using EventServiceProvider.Handlers;
using EventServiceProvider.Mappers;
using EventServiceProvider.Repositories;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
namespace EventServiceProvider.Services;

public interface IEventService
{
    Task<EventReply> AddEvent(Event request, ServerCallContext context);
    Task<EventReply> DeleteEvent(DeleteEventRequest request, ServerCallContext context);
    Task<GetEventReply> GetEventById(GetEventByIdRequest request, ServerCallContext context);
    Task<GetAllEventsReply> GetEvents(Empty request, ServerCallContext context);
    Task<EventReply> UpdateEvent(Event request, ServerCallContext context);
}

public class EventService(IEventRepository eventRepository, ICacheHandler<IEnumerable<Event>> cacheHandler) : EventContract.EventContractBase, IEventService
{
    private readonly IEventRepository _eventRepository = eventRepository;
    private readonly ICacheHandler<IEnumerable<Event>> _cacheHandler = cacheHandler;
    private const string _cacheKey = "events";

    public override async Task<GetAllEventsReply> GetEvents(Empty request, ServerCallContext context)
    {
        var cachedEvents = _cacheHandler.GetFromCache(_cacheKey);
        if (cachedEvents != null)
        {
            var reply = new GetAllEventsReply();
            reply.Events.AddRange(cachedEvents);
            return reply;
        }


        var freshReply = new GetAllEventsReply();
        var eventEntities = await _eventRepository.GetAllAsync(
            orderByDescending: false,
            sortBy: x => x.Date,
            filterBy: null,
            i => i.Category,
            i => i.BookingStatus
            );

        var events = eventEntities.Select(EventMapper.MapToGrpcEvent).ToList();
        _cacheHandler.SetCache(_cacheKey, events);

        freshReply.Events.AddRange(events);

        return freshReply;

    }
    public override async Task<GetEventReply> GetEventById(GetEventByIdRequest request, ServerCallContext context)
    {
        var cachedEvents = _cacheHandler.GetFromCache(_cacheKey);

        if (cachedEvents != null)
        {
            var cachedMatch = cachedEvents.FirstOrDefault(x => x.EventId == request.EventId);
            if (cachedMatch != null)
            {
                return new GetEventReply
                {
                    StatusCode = 200,
                    Event = cachedMatch
                };
            }
        }

        var eventEntity = await _eventRepository.GetAsync(
            x => x.EventId == request.EventId,
            i => i.Category,
            i => i.BookingStatus
            );
        if (eventEntity != null)
        {
            return new GetEventReply
            {
                StatusCode = 200,
                Event = EventMapper.MapToGrpcEvent(eventEntity)
            };
        }
        return new GetEventReply
        {
            StatusCode = 404,
        };
    }
    public override async Task<EventReply> AddEvent(Event request, ServerCallContext context)
    {
        var eventEntity = EventFactory.ToEntity(request);

        if (eventEntity == null)
        {
            return new EventReply
            {
                StatusCode = 400,
                Message = "Bad Request"
            };
        }
        var result = await _eventRepository.CreateAsync(eventEntity);

        if (result)
        {
            await UpdateCacheAsync();
            return new EventReply
            {
                StatusCode = 200,
                Message = "Event created successfully"
            };
        }
        else
        {
            return new EventReply
            {
                StatusCode = 500,
                Message = "Internal server error"
            };
        }

    }
    public override async Task<EventReply> UpdateEvent(Event request, ServerCallContext context)
    {
        var eventEntity = EventFactory.ToEntity(request);
        if (eventEntity == null)
        {
            return new EventReply
            {
                StatusCode = 400,
                Message = "Bad Request"
            };
        }

        var result = await _eventRepository.UpdateAsync(eventEntity);
        if (result == true)
        {
            await UpdateCacheAsync();
            return new EventReply
            {
                StatusCode = 200,
                Message = "Event updated successfully"
            };
        }
        else
        {
            return new EventReply
            {
                StatusCode = 404,
                Message = "Event not found"
            };
        }
    }

    public override async Task<EventReply> DeleteEvent(DeleteEventRequest request, ServerCallContext context)
    {
        try
        {
            var result = await _eventRepository.DeleteAsync(x => x.EventId == request.EventId);
            await UpdateCacheAsync();

            return new EventReply
            {
                StatusCode = 200,
                Message = "Event deleted successfully"
            };
        }
        catch
        {
            return new EventReply
            {
                StatusCode = 500,
                Message = "Internal server error"
            };
        }
    }

    private async Task<IEnumerable<Event>> UpdateCacheAsync()
    {
        var events = await _eventRepository.GetAllAsync(
            orderByDescending: false,
            sortBy: x => x.Date,
            filterBy: null,
            i => i.Category,
            i => i.BookingStatus            
            );
        var models = events.Select(EventMapper.MapToGrpcEvent).ToList();
        _cacheHandler.SetCache(_cacheKey, models);
        return models;
    }

}
