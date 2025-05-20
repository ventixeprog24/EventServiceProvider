using EventServiceProvider.Data;

namespace EventServiceProvider.Repositories;
public interface ICategoryRepository : IBaseRepository<CategoryEntity>
{
}
public class CategoryRepository(EventDbContext context) : BaseRepository<CategoryEntity>(context), ICategoryRepository
{
}