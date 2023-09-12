namespace EnsekTechTest.Domain.Entities
{
    public class MeterReading
    {
        public Guid Id { get; private set; }

        public DateTimeOffset ReadingDateTime { get; private set; }

        public int Value { get; private set; }

        public MeterReading(
            Guid id,
            DateTimeOffset readingDateTime,
            int value) 
        {
            Id = id;
            ReadingDateTime = readingDateTime;
            Value = value;
        }
    }
}
