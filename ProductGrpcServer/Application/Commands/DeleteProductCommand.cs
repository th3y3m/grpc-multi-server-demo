using MediatR;

namespace ProductGrpcServer.Application.Commands
{
    public record DeleteProductCommand(int Id) : IRequest;
}
