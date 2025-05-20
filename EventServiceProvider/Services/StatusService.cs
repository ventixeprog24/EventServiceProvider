using EventServiceProvider.Handlers;
using EventServiceProvider.Mappers;
using EventServiceProvider.Repositories;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;

namespace EventServiceProvider.Services;

public interface IStatusService
{
    Task<GetAllStatusesReply> GetStatuses(Empty request, ServerCallContext context);
}
public class StatusService(IStatusRepository statusRepository, ICacheHandler<IEnumerable<Status>> cacheHandler) : StatusContract.StatusContractBase, IStatusService
{
    private readonly IStatusRepository _statusRepository = statusRepository;
    private readonly ICacheHandler<IEnumerable<Status>> _cacheHandler = cacheHandler;
    private const string _cacheKey = "statuses";

    public override async Task<GetAllStatusesReply> GetStatuses(Empty request, ServerCallContext context)
    {
        var cachedStatuses = _cacheHandler.GetFromCache(_cacheKey);
        if (cachedStatuses != null)
        {
            var reply = new GetAllStatusesReply();
            reply.Statuses.AddRange(cachedStatuses);
            return reply;
        }
        var freshReply = new GetAllStatusesReply();
        var statusEntities = await _statusRepository.GetAllAsync(
            orderByDescending: false,
            sortBy: x => x.StatusName,
            filterBy: null
        );
        var statuses = statusEntities.Select(StatusMapper.MapToGrpcStatus).ToList();
        _cacheHandler.SetCache(_cacheKey, statuses);
        freshReply.Statuses.AddRange(statuses);
        return freshReply;
    }
}
