using CategoryGrpcServer.Application.Queries;
using Grpc.Core;
using GrpcContracts.Protos;
using MediatR;

namespace CategoryGrpcServer.API
{

    public class CategoryServiceImpl : CategoryService.CategoryServiceBase
    {
        private readonly IMediator _mediator;

        public CategoryServiceImpl(IMediator mediator)
        {
            _mediator = mediator;
        }

        public override async Task<Category> GetCategory(CategoryRequest request, ServerCallContext context)
        {
            var entity = await _mediator.Send(new GetCategoryByIdQuery(request.Id));
            if (entity == null)
                throw new RpcException(new Status(StatusCode.NotFound, "Category not found"));

            return new Category { Id = entity.Id, CategoryName = entity.CategoryName };
        }
    }
}
