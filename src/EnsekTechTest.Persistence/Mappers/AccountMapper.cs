using DomainAccount = EnsekTechTest.Domain.AggregateRoots.Account;
using DomainMeterReading = EnsekTechTest.Domain.Entities.MeterReading;
using PersistenceAccount = EnsekTechTest.Persistence.Models.Account;
using PersistenceMeterReading = EnsekTechTest.Persistence.Models.MeterReading;

namespace EnsekTechTest.Persistence.Mappers
{
    internal static class AccountMapper
    {
        public static DomainAccount Map(PersistenceAccount account)
        {
            if (account is null)
                return null;

            return new(
                id: account.Id,
                firstName: account.FirstName,
                lastName: account.LastName,
                meterReadings: (account.MeterReadings ?? Array.Empty<PersistenceMeterReading>()).Select(MeterReadingMapper.Map));
        }

        public static PersistenceAccount Map(DomainAccount account)
        {
            if (account is null)
                return null;

            return new PersistenceAccount
            {
                Id = account.Id,
                FirstName = account.FirstName,
                LastName = account.LastName,
                MeterReadings = (account.MeterReadings ?? Enumerable.Empty<DomainMeterReading>()).Select(MeterReadingMapper.Map).ToList()
            };
        }
    }
}
