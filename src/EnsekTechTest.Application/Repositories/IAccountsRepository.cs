using EnsekTechTest.Domain.AggregateRoots;

namespace EnsekTechTest.Application.Repositories
{
    public interface IAccountsRepository
    {
        Task<Account> GetByIdAsync(int id, CancellationToken cancellationToken);
    }
}
