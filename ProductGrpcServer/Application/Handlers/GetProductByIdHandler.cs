using GrpcContracts.Protos;
using MediatR;
using ProductGrpcServer.Application.Queries;
using ProductGrpcServer.Domain.Entities;
using ProductGrpcServer.Infrastructure.Persistence;
using GrpcContracts.Shared.Interfaces;

namespace CategoryGrpcServer.Application.Handlers
{
    public class GetProductByIdHandler : IRequestHandler<GetProductByIdQuery, ProductResponse?>
    {
        private readonly IUnitOfWork<AppDbContext> _unitOfWork;
        private readonly CategoryService.CategoryServiceClient _categoryClient;

        public GetProductByIdHandler(IUnitOfWork<AppDbContext> unitOfWork, CategoryService.CategoryServiceClient categoryClient)
        {
            _unitOfWork = unitOfWork;
            _categoryClient = categoryClient;
        }

        public async Task<ProductResponse?> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
        {
            var product = await _unitOfWork.Repository<Product>().GetByIdAsync(request.Id, cancellationToken);
            if (product == null)
                throw new Exception("Product not found");

            var categoryResponse = await _categoryClient.GetCategoryAsync(new CategoryRequest { Id = product.CategoryId });

            return new ProductResponse
            {
                Id = product.Id,
                ProductName = product.ProductName,
                CategoryName = categoryResponse.CategoryName
            };
        }
    }

}
