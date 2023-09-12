namespace EnsekTechTest.Domain.Entities
{
    public class MeterReading
    {
        public DateTimeOffset ReadingDateTime { get; private set; }

        public int Value { get; private set; }

        public MeterReading(
            DateTimeOffset readingDateTime,
            int value) 
        {
            ReadingDateTime = readingDateTime;
            Value = value;
        }
    }
}
