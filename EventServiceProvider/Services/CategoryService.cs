using EventServiceProvider.Handlers;
using EventServiceProvider.Mappers;
using EventServiceProvider.Repositories;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;

namespace EventServiceProvider.Services;

public interface ICategoryService
{
    Task<GetAllCategoriesReply> GetCategories(Empty request, ServerCallContext context);
}

public class CategoryService(ICategoryRepository categoryRepository, ICacheHandler<IEnumerable<Category>> cacheHandler) : CategoryContract.CategoryContractBase, ICategoryService
{
    private readonly ICategoryRepository _categoryRepository = categoryRepository;
    private readonly ICacheHandler<IEnumerable<Category>> _cacheHandler = cacheHandler;
    private const string _cacheKey = "categories";

    public override async Task<GetAllCategoriesReply> GetCategories(Empty request, ServerCallContext context)
    {
        var cachedCategories = _cacheHandler.GetFromCache(_cacheKey);
        if (cachedCategories != null)
        {
            var reply = new GetAllCategoriesReply();
            reply.Categories.AddRange(cachedCategories);
            return reply;
        }
        var freshReply = new GetAllCategoriesReply();
        var categoryEntities = await _categoryRepository.GetAllAsync(
            orderByDescending: false,
            sortBy: x => x.CategoryName,
            filterBy: null
        );
        var categories = categoryEntities.Select(CategoryMapper.MapToGrpcCategory).ToList();
        _cacheHandler.SetCache(_cacheKey, categories);
        freshReply.Categories.AddRange(categories);
        return freshReply;
    }
}
