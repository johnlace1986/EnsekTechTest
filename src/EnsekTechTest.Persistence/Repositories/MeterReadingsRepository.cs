using EnsekTechTest.Application.Infrastructure.Repositories;
using EnsekTechTest.Persistence.DbContexts;
using EnsekTechTest.Persistence.Models;

namespace EnsekTechTest.Persistence.Repositories
{
    internal class MeterReadingsRepository : IMeterReadingsRepository
    {
        private readonly IMeterReadingContext _meterReadingContext;

        public MeterReadingsRepository(IMeterReadingContext meterReadingContext)
        {
            _meterReadingContext = meterReadingContext;
        }

        public async Task AddMeterReadingAsync(int accountId, DateTimeOffset readingDateTime, int value, CancellationToken cancellationToken)
        {
            await _meterReadingContext.MeterReadings.AddAsync(new MeterReading
            {
                ReadingDateTime = readingDateTime,
                Value = value,
                AccountId = accountId
            }, cancellationToken);
        }
    }
}
