using ApiGateway.CacheRedis;
using ApiGateway.CacheRedis.Cache;
using ApiGateway.ProxyService.Category;
using GrpcContracts.Protos;

namespace ApiGateway.Business.Category
{
    public class CategoryBusiness : ICategoryBusiness
    {
        private readonly ICacheRedis _cache;
        private readonly ICategoryServiceProxy _categoryServiceProxy;

        public CategoryBusiness(ICacheRedis cache, ICategoryServiceProxy categoryServiceProxy)
        {
            _cache = cache;
            _categoryServiceProxy = categoryServiceProxy;
        }

        public async Task<GrpcContracts.Protos.Category> GetCategoryByIdAsync(int id)
        {
            var cacheKey = $"{CacheKeys.Key_Category_By_Id}{id}";
            GrpcContracts.Protos.Category category = null;

            try
            {
                category = await _cache.GetAsync<GrpcContracts.Protos.Category>(cacheKey);
                if (category != null)
                {
                    return category;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Redis unavailable: {ex.Message}");
            }

            category = await _categoryServiceProxy.GetCategoryByIdAsync(id);
            if (category == null)
            {
                throw new Exception("Category not found");
            }

            try
            {
                await _cache.SetAsync(cacheKey, category, TimeSpan.FromDays(1));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to cache category: {ex.Message}");
            }

            return category;
        }

    }
}
