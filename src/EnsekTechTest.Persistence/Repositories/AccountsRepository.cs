using EnsekTechTest.Application.Repositories;
using EnsekTechTest.Persistence.Mappers;
using Microsoft.EntityFrameworkCore;
using DomainAccount = EnsekTechTest.Domain.AggregateRoots.Account;

namespace EnsekTechTest.Persistence.Repositories
{
    public class AccountsRepository : IAccountsRepository
    {
        private readonly AccountContext _context;

        public AccountsRepository()
        {
            _context = new AccountContext();
        }

        public async Task<DomainAccount> GetByIdAsync(int id, CancellationToken cancellationToken)
        {
            var account = await _context.Accounts
                .Include(account => account.MeterReadings)
                .SingleOrDefaultAsync(account => account.Id == id, cancellationToken);

            return AccountMapper.Map(account);
        }     
    }
}
