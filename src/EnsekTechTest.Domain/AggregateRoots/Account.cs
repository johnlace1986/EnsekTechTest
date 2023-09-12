using EnsekTechTest.Core;
using EnsekTechTest.Domain.Entities;
using EnsekTechTest.Domain.Failures;

namespace EnsekTechTest.Domain.AggregateRoots
{
    public class Account
    {
        public int Id { get; private set; }

        public string FirstName { get; private set; }

        public string LastName { get; private set; }

        public IEnumerable<MeterReading> MeterReadings { get; private set; }

        public Account(
            int id,
            string firstName,
            string lastName,
            IEnumerable<MeterReading> meterReadings)
        {
            Id = id;
            FirstName = firstName;
            LastName = lastName;
            MeterReadings = meterReadings;
        }

        public Result<Unit, AddMeterReadingToAccountFailure> AddMeterReading(DateTimeOffset readingDateTime, int value)
        {
            if (MeterReadings.Any(meterReading => meterReading.ReadingDateTime == readingDateTime && meterReading.Value == value))
                return AddMeterReadingToAccountFailure.AlreadyAdded;

            if (MeterReadings.Any(meterReading => meterReading.ReadingDateTime >= readingDateTime))
                return AddMeterReadingToAccountFailure.NewerReadingExists;

            if (ValueIsInRange(value) is false)
                return AddMeterReadingToAccountFailure.ValueOutOfRange;

            var meterReadings = MeterReadings.ToList();
            meterReadings.Add(new MeterReading(readingDateTime, value));

            MeterReadings = meterReadings;
            return Unit.Instance;
        }

        private static bool ValueIsInRange(int value) => -99999 <= value && value <= 99999;
    }
}
