using CategoryGrpcServer.Domain.Entities;
using MediatR;

namespace CategoryGrpcServer.Application.Queries
{
    public class GetCategoryByIdQuery : IRequest<Category?>
    {
        public int Id { get; set; }

        public GetCategoryByIdQuery(int id) => Id = id;
    }
}
