using GrpcContracts.Protos;
using MediatR;
using FluentValidation;

namespace ProductGrpcServer.Application.Commands
{
    public record CreateProductCommand(string ProductName, int CategoryId) : IRequest<ProductResponse>;

    public class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
    {
        public CreateProductCommandValidator()
        {
            RuleFor(x => x.ProductName)
                .NotEmpty()
                .WithMessage("Product Name cannot be empty.");
            RuleFor(x => x.CategoryId)
                .GreaterThan(0)
                .WithMessage("Category Id must be greater than zero.");
        }
    }
}
