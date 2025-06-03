using GrpcContracts.Protos;
using MediatR;

namespace ProductGrpcServer.Application.Queries
{
    public record GetAllProductsQuery() : IRequest<List<ProductResponse>>;
}
