using EnsekTechTest.Application.Repositories;
using EnsekTechTest.Persistence.Models;

namespace EnsekTechTest.Persistence.Repositories
{
    public class MeterReadingsRepository : IMeterReadingsRepository
    {
        private readonly AccountContext _context;

        public MeterReadingsRepository()
        {
            _context = new AccountContext();
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
