using GrpcContracts.Protos;
using MediatR;
using FluentValidation;

namespace ProductGrpcServer.Application.Queries
{
    public class GetProductByIdQuery : IRequest<ProductResponse?>
    {
        public int Id { get; set; }

        public GetProductByIdQuery(int id) => Id = id;
    }

    public class GetProductByIdQueryValidator : AbstractValidator<GetProductByIdQuery>
    {
        public GetProductByIdQueryValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0)
                .WithMessage("Product Id must be greater than zero.");
        }
    }
}
