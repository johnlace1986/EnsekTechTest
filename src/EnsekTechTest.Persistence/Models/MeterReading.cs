using System.ComponentModel.DataAnnotations.Schema;

namespace EnsekTechTest.Persistence.Models
{
    public class MeterReading
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        public DateTimeOffset MeterReadingDateTime { get; set; }

        public int MeterReadValue { get; set; }

        public virtual Account Account { get; set; }
    }
}
