using GrpcContracts.Protos;
using MediatR;
using FluentValidation;

namespace ProductGrpcServer.Application.Commands
{
    public record UpdateProductCommand(int Id, string ProductName, int CategoryId) : IRequest<ProductResponse>;

    public class UpdateProductCommandValidator : AbstractValidator<UpdateProductCommand>
    {
        public UpdateProductCommandValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0)
                .WithMessage("Product Id must be greater than zero.");
            RuleFor(x => x.ProductName)
                .NotEmpty()
                .WithMessage("Product Name cannot be empty.");
            RuleFor(x => x.CategoryId)
                .GreaterThan(0)
                .WithMessage("Category Id must be greater than zero.");
        }
    }
}
