using GrpcContracts.Protos;
using GrpcContracts.Shared.Interfaces;
using MediatR;
using ProductGrpcServer.Application.Queries;
using ProductGrpcServer.Domain.Entities;
using ProductGrpcServer.Infrastructure.Persistence;

namespace ProductGrpcServer.Application.Handlers
{
    public class GetAllProductsHandler : IRequestHandler<GetAllProductsQuery, List<ProductResponse>>
    {
        private readonly IUnitOfWork<AppDbContext> _unitOfWork;
        private readonly CategoryService.CategoryServiceClient _categoryClient;

        public GetAllProductsHandler(IUnitOfWork<AppDbContext> unitOfWork, CategoryService.CategoryServiceClient categoryClient)
        {
            _unitOfWork = unitOfWork;
            _categoryClient = categoryClient;
        }

        public async Task<List<ProductResponse>> Handle(GetAllProductsQuery request, CancellationToken cancellationToken)
        {
            var products = await _unitOfWork.Repository<Product>().ListAllAsync(cancellationToken);
            var result = new List<ProductResponse>();

            foreach (var p in products)
            {
                var cat = await _categoryClient.GetCategoryAsync(new CategoryRequest { Id = p.CategoryId });

                result.Add(new ProductResponse
                {
                    Id = p.Id,
                    ProductName = p.ProductName,
                    CategoryName = cat.CategoryName
                });
            }

            return result;
        }
    }
}
