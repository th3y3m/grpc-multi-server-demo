using ApiGateway.ProxyService.Base;
using GrpcContracts.Protos;

namespace ApiGateway.ProxyService.Category
{
    public class CategoryServiceProxy : BaseProxy, ICategoryServiceProxy
    {
        private readonly CategoryService.CategoryServiceClient _categoryClient;

        public CategoryServiceProxy(
            CategoryService.CategoryServiceClient categoryClient)
            : base(typeof(CategoryServiceProxy))
        {
            _categoryClient = categoryClient ?? throw new ArgumentNullException(nameof(categoryClient));
        }

        public async Task<GrpcContracts.Protos.Category> GetCategoryByIdAsync(int categoryId)
        {
            try
            {
                var request = new CategoryRequest { Id = categoryId };
                var response = await _categoryClient.GetCategoryAsync(request);
                return response;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error fetching category with ID {categoryId}: {ex.Message}", ex);
            }
        }
    }
}