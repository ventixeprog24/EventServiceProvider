using EventServiceProvider.Mappers;
using EventServiceProvider.Repository;
using Grpc.Core;
namespace EventServiceProvider.Services;

public class EventService(EventRepository eventRepository) : EventContract.EventContractBase
{
    private readonly EventRepository _eventRepository = eventRepository;
    public override async Task<GetEventsReply> GetEvents(GetEventsRequest request, ServerCallContext context)
    {

        var reply = new GetEventsReply();
        var eventEntities = await _eventRepository.GetAllEventsAsync();
        var events = eventEntities.Select(EventMapper.MapToGrpcEvent).ToList();
        reply.Events.AddRange(events);

        return reply;
    }

}
