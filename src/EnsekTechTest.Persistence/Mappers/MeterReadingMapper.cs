using DomainMeterReading = EnsekTechTest.Domain.Entities.MeterReading;
using PersistenceMeterReading = EnsekTechTest.Persistence.Models.MeterReading;

namespace EnsekTechTest.Persistence.Mappers
{
    internal static class MeterReadingMapper
    {
        public static DomainMeterReading Map(PersistenceMeterReading meterReading)
        {
            if (meterReading is null)
                return null;

            return new(meterReading.ReadingDateTime, meterReading.Value);
        }

        public static PersistenceMeterReading Map(DomainMeterReading meterReading)
        {
            if (meterReading is null)
                return null;

            return new()
            {
                ReadingDateTime = meterReading.ReadingDateTime,
                Value = meterReading.Value
            };
        }
    }
}
