using EnsekTechTest.Application.Infrastructure.Repositories;
using EnsekTechTest.Persistence.DbContexts;
using EnsekTechTest.Persistence.Mappers;
using Microsoft.EntityFrameworkCore;
using DomainAccount = EnsekTechTest.Domain.AggregateRoots.Account;

namespace EnsekTechTest.Persistence.Repositories
{
    internal class AccountsRepository : IAccountsRepository
    {
        private readonly IAccountContext _accountContext;

        public AccountsRepository(IAccountContext accountContext)
        {
            _accountContext = accountContext;
        }

        public async Task<DomainAccount> GetByIdAsync(int id, CancellationToken cancellationToken)
        {
            var account = await _accountContext.Accounts
                .Include(account => account.MeterReadings)
                .SingleOrDefaultAsync(account => account.Id == id, cancellationToken);

            return AccountMapper.Map(account);
        }     
    }
}
