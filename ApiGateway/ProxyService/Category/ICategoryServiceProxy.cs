namespace ApiGateway.ProxyService.Category
{
    public interface ICategoryServiceProxy
    {
        Task<GrpcContracts.Protos.Category> GetCategoryByIdAsync(int categoryId);
    }
}
