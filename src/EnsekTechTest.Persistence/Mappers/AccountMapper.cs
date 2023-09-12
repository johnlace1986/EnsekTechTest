using DomainAccount = EnsekTechTest.Domain.AggregateRoots.Account;
using DomainMeterReading = EnsekTechTest.Domain.Entities.MeterReading;
using PersistenceAccount = EnsekTechTest.Persistence.Models.Account;
using PersistenceMeterReading = EnsekTechTest.Persistence.Models.MeterReading;

namespace EnsekTechTest.Persistence.Mappers
{
    public static class AccountMapper
    {
        public static DomainAccount Map(PersistenceAccount account)
        {
            if (account is null)
                return null;

            return new(
                id: account.Id,
                firstName: account.FirstName,
                lastName: account.LastName,
                meterReadings: (account.MeterReadings ?? Array.Empty<PersistenceMeterReading>()).Select(Map));
        }

        public static DomainMeterReading Map(PersistenceMeterReading meterReading)
        {
            if (meterReading is null)
                return null;

            return new(meterReading.Id, meterReading.ReadingDateTime, meterReading.Value);
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
                MeterReadings = (account.MeterReadings ?? Enumerable.Empty<DomainMeterReading>()).Select(Map).ToList()
            };
        }

        public static PersistenceMeterReading Map(DomainMeterReading meterReading)
        {
            if (meterReading is null)
                return null;

            return new()
            {
                Id = meterReading.Id,
                ReadingDateTime = meterReading.ReadingDateTime,
                Value = meterReading.Value
            };
        }
    }
}
