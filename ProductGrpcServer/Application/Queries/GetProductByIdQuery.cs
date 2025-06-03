using GrpcContracts.Protos;
using MediatR;

namespace ProductGrpcServer.Application.Queries
{
    public class GetProductByIdQuery : IRequest<ProductResponse?>
    {
        public int Id { get; set; }

        public GetProductByIdQuery(int id) => Id = id;
    }
}
