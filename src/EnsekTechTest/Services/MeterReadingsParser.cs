namespace EnsekTechTest.Services
{
    public class MeterReadingsParser : IMeterReadingsParser
    {
        public async Task<IEnumerable<MeterReading>> ParseMeterReadings(Stream stream, CancellationToken cancellationToken)
        {
            using var streamReader = new StreamReader(stream);

            //header row
            await streamReader.ReadLineAsync(cancellationToken);

            var meterReadings = new List<MeterReading>();

            while (streamReader.EndOfStream is false)
            {
                var line = await streamReader.ReadLineAsync(cancellationToken);
                var parts = line?.Split(',')!;

                meterReadings.Add(new MeterReading
                {
                    AccountId = int.Parse(parts[0]),
                    ReadingDateTime = DateTimeOffset.Parse(parts[1]),
                    Value = int.Parse(parts[2])
                });
            }

            return meterReadings;
        }
    }
}
