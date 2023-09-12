namespace EnsekTechTest.Domain.Entities
{
    public class MeterReading
    {
        public Guid Id { get; private set; }

        public DateTimeOffset ReadingDateTime { get; private set; }

        public int Value { get; private set; }

        public MeterReading(
            Guid id,
            DateTimeOffset readDateTime,
            int value) 
        {
            Id = id;
            ReadingDateTime = readDateTime;
            Value = value;
        }
    }
}
