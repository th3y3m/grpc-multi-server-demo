using GrpcContracts.Protos;
using MediatR;

namespace ProductGrpcServer.Application.Commands
{
    public record CreateProductCommand(string ProductName, int CategoryId) : IRequest<ProductResponse>;
}
