using GrpcContracts.Protos;
using MediatR;

namespace ProductGrpcServer.Application.Commands
{
    public record UpdateProductCommand(int Id, string ProductName, int CategoryId) : IRequest<ProductResponse>;
}
