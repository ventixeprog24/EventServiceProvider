using EventServiceProvider.Data;

namespace EventServiceProvider.Mappers;

public class StatusMapper
{
    public static Status MapToGrpcStatus(StatusEntity statusEntity)
    {
        return new Status
        {
            StatusId = statusEntity.StatusId,
            StatusName = statusEntity.StatusName,
        };
    }
}
