namespace EnsekTechTest.Services
{
    public interface IMeterReadingsParser
    {
        public Task<IEnumerable<MeterReading>> ParseMeterReadings(Stream stream, CancellationToken cancellationToken);
    }

    public class MeterReading
    {
        public int AccountId { get; set; }

        public DateTimeOffset ReadingDateTime { get; set; }

        public int Value { get; set; }
    }
}
