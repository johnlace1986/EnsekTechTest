using EnsekTechTest.Application.Infrastructure.Repositories;
using EnsekTechTest.Persistence.DbContexts;
using EnsekTechTest.Persistence.Models;

namespace EnsekTechTest.Persistence.Repositories
{
    internal class MeterReadingsRepository : IMeterReadingsRepository
    {
        private readonly PersistenceContext _context;

        public MeterReadingsRepository()
        {
            _context = new PersistenceContext();
        }

        public async Task AddMeterReadingAsync(int accountId, DateTimeOffset readingDateTime, int value, CancellationToken cancellationToken)
        {
            await _context.MeterReadings.AddAsync(new MeterReading
            {
                ReadingDateTime = readingDateTime,
                Value = value,
                AccountId = accountId
            }, cancellationToken);

            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
