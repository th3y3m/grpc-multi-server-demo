using GrpcContracts.Shared.Interfaces;
using MediatR;
using ProductGrpcServer.Application.Commands;
using ProductGrpcServer.Domain.Entities;
using ProductGrpcServer.Infrastructure.Persistence;

namespace ProductGrpcServer.Application.Handlers
{
    public class DeleteProductHandler : IRequestHandler<DeleteProductCommand>
    {
        private readonly IUnitOfWork<AppDbContext> _unitOfWork;

        public DeleteProductHandler(IUnitOfWork<AppDbContext> unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task Handle(DeleteProductCommand request, CancellationToken cancellationToken)
        {
            var repo = _unitOfWork.Repository<Product>();
            var product = await repo.GetByIdAsync(request.Id, cancellationToken);

            if (product == null)
                throw new Exception("Product not found");

            repo.Delete(product);
            await _unitOfWork.CommitAsync();
        }
    }
}
