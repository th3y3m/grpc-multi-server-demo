using CategoryGrpcServer.Application.Queries;
using CategoryGrpcServer.Domain.Entities;
using CategoryGrpcServer.Infrastructure.Persistence;
using GrpcContracts.Shared.Interfaces;
using MediatR;

namespace CategoryGrpcServer.Application.Handlers
{
    public class GetCategoryByIdHandler : IRequestHandler<GetCategoryByIdQuery, Category?>
    {
        private readonly IUnitOfWork<AppDbContext> _unitOfWork;

        public GetCategoryByIdHandler(IUnitOfWork<AppDbContext> unitOfWork) => _unitOfWork = unitOfWork;

        public async Task<Category?> Handle(GetCategoryByIdQuery request, CancellationToken cancellationToken)
        {
            return await _unitOfWork.Repository<Category>().GetByIdAsync(request.Id, cancellationToken);
        }
    }
}
