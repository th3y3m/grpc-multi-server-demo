using Microsoft.EntityFrameworkCore;

namespace GrpcContracts.Shared.Interfaces
{
    public interface IUnitOfWork<TContext> : IDisposable where TContext : DbContext
    {
        IGenericRepository<T, TContext> Repository<T>() where T : class;
        Task<int> CommitAsync(CancellationToken cancellationToken = default);
    }
}
