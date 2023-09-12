namespace EnsekTechTest.Application.Repositories
{
    public interface IMeterReadingsRepository
    {
        Task AddMeterReadingAsync(int accountId, DateTimeOffset readingDateTime, int value, CancellationToken cancellationToken);
    }
}
