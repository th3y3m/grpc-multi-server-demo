using Microsoft.EntityFrameworkCore;

namespace GrpcContracts.Shared.Interfaces
{
    public interface IGenericRepository<T, TContext> 
        where T : class 
        where TContext : DbContext
    {
        Task<IReadOnlyList<T>> ListAllAsync(CancellationToken cancellationToken = default);
        Task<T> GetByIdAsync(int id, CancellationToken cancellationToken = default);
        Task<T> GetEntityWithSpec(ISpecification<T> spec);
        Task<IReadOnlyList<T>> ListAsync(ISpecification<T> spec);
        Task<int> CountAsync(ISpecification<T> spec);
        void Add(T entity);
        void Update(T entity);
        void Delete(T entity);
        Task<bool> SaveAllAsync(CancellationToken cancellationToken = default);
        void Attach(T t);
    }
}
