namespace EnsekTechTest.Persistence.Models
{
    public class MeterReading
    {
        public Guid Id { get; set; }

        public DateTimeOffset ReadingDateTime { get; set; }

        public int Value { get; set; }

        public virtual Account Account { get; set; }
    }
}
