using GrpcContracts.Protos;
using GrpcContracts.Shared.Interfaces;
using MediatR;
using ProductGrpcServer.Application.Commands;
using ProductGrpcServer.Domain.Entities;
using ProductGrpcServer.Infrastructure.Persistence;

namespace ProductGrpcServer.Application.Handlers
{
    public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, ProductResponse>
    {
        private readonly IUnitOfWork<AppDbContext> _unitOfWork;
        private readonly CategoryService.CategoryServiceClient _categoryClient;

        public CreateProductCommandHandler(
            IUnitOfWork<AppDbContext> unitOfWork,
            CategoryService.CategoryServiceClient categoryClient)
        {
            _unitOfWork = unitOfWork;
            _categoryClient = categoryClient;
        }

        public async Task<ProductResponse> Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {
            var product = new Product
            {
                ProductName = request.ProductName,
                CategoryId = request.CategoryId
            };

            _unitOfWork.Repository<Product>().Add(product);
            await _unitOfWork.CommitAsync();

            var category = await _categoryClient.GetCategoryAsync(new CategoryRequest { Id = product.CategoryId });

            return new ProductResponse
            {
                Id = product.Id,
                ProductName = product.ProductName,
                CategoryName = category.CategoryName
            };
        }
    }
}
