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

        public Result<Unit, AddMeterReadingToAccountFailure> AddMeterReading(DateTimeOffset readDateTime, int value)
        {
            if (MeterReadings.Any(meterReading => meterReading.ReadingDateTime == readDateTime && meterReading.Value == value))
                return AddMeterReadingToAccountFailure.AlreadyAdded;

            if (MeterReadings.Any(meterReading => meterReading.ReadingDateTime >= readDateTime))
                return AddMeterReadingToAccountFailure.NewerReadingExists;

            var meterReadings = MeterReadings.ToList();
            meterReadings.Add(new MeterReading(Guid.NewGuid(), readDateTime, value));

            MeterReadings = meterReadings;
            return Unit.Instance;
        }
    }
}
