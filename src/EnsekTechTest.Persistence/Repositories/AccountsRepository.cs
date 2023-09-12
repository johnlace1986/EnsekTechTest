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

        public Task CommitChanges(DomainAccount domainAccount, CancellationToken cancellationToken)
        {
            var persistenceAccount = _context.Accounts.Include(account => account.MeterReadings).SingleOrDefault(account => account.Id == domainAccount.Id);

            if (persistenceAccount == null)
                _context.Add(AccountMapper.Map(domainAccount));
            else
            {
                persistenceAccount.FirstName = domainAccount.FirstName;
                persistenceAccount.LastName = domainAccount.LastName;
                persistenceAccount.MeterReadings = domainAccount.MeterReadings.Select(AccountMapper.Map).ToList();

                _context.Accounts.Update(persistenceAccount);
            }

            return _context.SaveChangesAsync(cancellationToken);
        }        
    }
}
