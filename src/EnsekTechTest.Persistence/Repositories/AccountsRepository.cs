using EnsekTechTest.Application.Repositories;
using Microsoft.EntityFrameworkCore;
using DomainAccount = EnsekTechTest.Domain.AggregateRoots.Account;
using DomainMeterReading = EnsekTechTest.Domain.Entities.MeterReading;
using PersistenceAccount = EnsekTechTest.Persistence.Models.Account;
using PersistenceMeterReading = EnsekTechTest.Persistence.Models.MeterReading;

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
            return Map(await _context.Accounts.Include(account => account.MeterReadings).SingleOrDefaultAsync(account => account.Id == id, cancellationToken));
        }

        private DomainAccount Map(PersistenceAccount account) =>
            new(
                id: account.Id, 
                firstName: account.FirstName, 
                lastName: account.LastName, 
                meterReadings: (account.MeterReadings ?? Array.Empty<PersistenceMeterReading>()).Select(Map));

        private DomainMeterReading Map(PersistenceMeterReading meterReading) =>
            new(meterReading.Id, meterReading.ReadingDateTime, meterReading.Value);

        private PersistenceAccount Map(DomainAccount account)
        {
            var persistenceAccount = new PersistenceAccount
            {
                Id = account.Id,
                FirstName = account.FirstName,
                LastName = account.LastName,
            };

            persistenceAccount.MeterReadings = (account.MeterReadings ?? Enumerable.Empty<DomainMeterReading>()).Select(meterReading => Map(meterReading, persistenceAccount)).ToList();

            return persistenceAccount;
        }

        private PersistenceMeterReading Map(DomainMeterReading meterReading, PersistenceAccount persistenceAccount)
        {
            return new()
            {
                Id = meterReading.Id,
                ReadingDateTime = meterReading.ReadingDateTime,
                Value = meterReading.Value,
                Account = persistenceAccount
            };
        }

        public Task CommitChanges(DomainAccount domainAccount, CancellationToken cancellationToken)
        {
            var persistenceAccount = _context.Accounts.SingleOrDefault(account => account.Id == domainAccount.Id);

            if (persistenceAccount == null)
                _context.Add(Map(domainAccount));
            else
            {
                persistenceAccount.FirstName = domainAccount.FirstName;
                persistenceAccount.LastName = domainAccount.LastName;
                persistenceAccount.MeterReadings = domainAccount.MeterReadings.Select(meterReading => Map(meterReading, persistenceAccount)).ToList();

                _context.Update(persistenceAccount);
            }

            return _context.SaveChangesAsync(cancellationToken);
        }        
    }
}
