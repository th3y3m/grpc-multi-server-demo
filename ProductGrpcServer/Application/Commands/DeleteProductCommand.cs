using MediatR;
using FluentValidation;

namespace ProductGrpcServer.Application.Commands
{
    public record DeleteProductCommand(int Id) : IRequest;

    public class DeleteProductCommandValidator : AbstractValidator<DeleteProductCommand>
    {
        public DeleteProductCommandValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0)
                .WithMessage("Product Id must be greater than zero.");
        }
    }
}
