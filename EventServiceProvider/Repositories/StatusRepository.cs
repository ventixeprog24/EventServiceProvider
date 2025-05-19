using EventServiceProvider.Data;

namespace EventServiceProvider.Repositories;
internal interface IStatusRepository : IBaseRepository<StatusEntity>
{
}
public class StatusRepository(EventDbContext context) : BaseRepository<StatusEntity>(context), IStatusRepository
{
}
