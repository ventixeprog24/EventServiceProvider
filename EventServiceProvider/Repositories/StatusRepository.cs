using EventServiceProvider.Data;

namespace EventServiceProvider.Repositories;
public interface IStatusRepository : IBaseRepository<StatusEntity>
{
}
public class StatusRepository(EventDbContext context) : BaseRepository<StatusEntity>(context), IStatusRepository
{
}
