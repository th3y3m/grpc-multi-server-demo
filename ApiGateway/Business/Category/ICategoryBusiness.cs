namespace ApiGateway.Business.Category
{
    public interface ICategoryBusiness
    {
        Task<GrpcContracts.Protos.Category> GetCategoryByIdAsync(int categoryId);
    }
}
