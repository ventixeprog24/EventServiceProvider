using EventServiceProvider.Data;

namespace EventServiceProvider.Repositories;
internal interface ICategoryRepository : IBaseRepository<CategoryEntity>
{
}
public class CategoryRepository(EventDbContext context) : BaseRepository<CategoryEntity>(context), ICategoryRepository
{
}