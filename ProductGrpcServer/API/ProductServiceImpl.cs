using Grpc.Core;
using GrpcContracts.Protos;
using MediatR;
using ProductGrpcServer.Application.Commands;
using ProductGrpcServer.Application.Queries;

namespace ProductGrpcServer.API
{
    public class ProductServiceImpl : ProductService.ProductServiceBase
    {
        private readonly IMediator _mediator;

        public ProductServiceImpl(IMediator mediator)
        {
            _mediator = mediator;
        }

        public override async Task<ProductResponse> GetProductById(ProductRequest request, ServerCallContext context)
        {
            var dto = await _mediator.Send(new GetProductByIdQuery(request.Id));
            return dto;
        }

        public override async Task<ProductList> GetAllProducts(Empty request, ServerCallContext context)
        {
            var products = await _mediator.Send(new GetAllProductsQuery());
            var list = new ProductList();
            list.Products.AddRange(products.Select(p => new ProductResponse
            {
                Id = p.Id,
                ProductName = p.ProductName,
                CategoryName = p.CategoryName
            }));
            return list;
        }

        public override async Task<ProductResponse> CreateProduct(CreateProductRequest request, ServerCallContext context)
        {
            var dto = await _mediator.Send(new CreateProductCommand(
                request.ProductName,
                request.CategoryId
            ));
            return dto;
        }

        public override async Task<ProductResponse> UpdateProduct(UpdateProductRequest request, ServerCallContext context)
        {
            var dto = await _mediator.Send(new UpdateProductCommand(
                request.Id,
                request.ProductName,
                request.CategoryId
            ));
            return dto;
        }

        public override async Task<Empty> DeleteProduct(DeleteProductRequest request, ServerCallContext context)
        {
            await _mediator.Send(new DeleteProductCommand(request.Id));
            return new Empty();
        }
    }
}