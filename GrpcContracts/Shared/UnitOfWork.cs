using GrpcContracts.Shared.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Concurrent;

namespace GrpcContracts.Shared
{
    public class UnitOfWork<TContext> : IUnitOfWork<TContext> where TContext : DbContext
    {
        private readonly TContext _context;
        private readonly ConcurrentDictionary<string, object> _repositories = new();

        public UnitOfWork(TContext context)
        {
            _context = context;
        }

        public void Dispose() => _context.Dispose();

        public Task<int> CommitAsync(CancellationToken cancellationToken = default)
        {
            return _context.SaveChangesAsync(cancellationToken);
        }

        public IGenericRepository<TEntity, TContext> Repository<TEntity>() where TEntity : class
        {
            var type = typeof(TEntity).Name;

            return (IGenericRepository<TEntity, TContext>)_repositories.GetOrAdd(type, t =>
            {
                var repoType = typeof(GenericRepository<,>).MakeGenericType(typeof(TEntity), typeof(TContext));
                return Activator.CreateInstance(repoType, _context)
                    ?? throw new InvalidOperationException(
                        $"Could not create repository instance for {t}");
            });
        }
    }
}
