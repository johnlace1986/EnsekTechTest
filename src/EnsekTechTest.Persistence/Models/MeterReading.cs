using System.ComponentModel.DataAnnotations.Schema;

namespace EnsekTechTest.Persistence.Models
{
    public class MeterReading
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public Guid Id { get; set; }

        public DateTimeOffset ReadingDateTime { get; set; }

        public int Value { get; set; }

        public virtual Account Account { get; set; }
    }
}
