using GrpcContracts.Protos;
using GrpcContracts.Shared.Interfaces;
using MediatR;
using ProductGrpcServer.Application.Commands;
using ProductGrpcServer.Domain.Entities;
using ProductGrpcServer.Infrastructure.Persistence;

namespace ProductGrpcServer.Application.Handlers
{
    public class UpdateProductHandler : IRequestHandler<UpdateProductCommand, ProductResponse>
    {
        private readonly IUnitOfWork<AppDbContext> _unitOfWork;
        private readonly CategoryService.CategoryServiceClient _categoryClient;

        public UpdateProductHandler(IUnitOfWork<AppDbContext> unitOfWork, CategoryService.CategoryServiceClient categoryClient)
        {
            _unitOfWork = unitOfWork;
            _categoryClient = categoryClient;
        }

        public async Task<ProductResponse> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
        {
            var _repo = _unitOfWork.Repository<Product>();
            var product = await _repo.GetByIdAsync(request.Id, cancellationToken);

            if (product == null) throw new Exception("Product not found");

            product.ProductName = request.ProductName;
            product.CategoryId = request.CategoryId;

            _repo.Update(product);
            await _unitOfWork.CommitAsync(cancellationToken);

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
