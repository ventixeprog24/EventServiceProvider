using EventServiceProvider.Data;

namespace EventServiceProvider.Mappers;

public class CategoryMapper
{
    public static Category MapToGrpcCategory(CategoryEntity categoryEntity)
    {
        return new Category
        {
            CategoryId = categoryEntity.CategoryId,
            CategoryName = categoryEntity.CategoryName,
        };
    }
}
