using EnsekTechTest.Domain.AggregateRoots;
using EnsekTechTest.Domain.Entities;
using FluentAssertions;
using FluentAssertions.Execution;
using NUnit.Framework;

namespace EnsekTechTest.Domain.Tests.AggregateRoots
{
    public class AccountTests
    {
        [Test]
        public void MeterReadingAlreadyAdded()
        {
            var readingDateTime = DateTimeOffset.UtcNow;
            var value = 12345;

            var account = new Account(1, "Joe", "Bloggs", new[]
            {
                new MeterReading(Guid.NewGuid(), readingDateTime, value)
            });

            var result = account.AddMeterReading(readingDateTime, value);

            using (new AssertionScope())
            {
                result.IsSuccess.Should().BeFalse();
                result.Failure.Should().Be(Failures.AddMeterReadingToAccountFailure.AlreadyAdded);
            }
        }

        [Test]
        public void NewerReadingExists()
        {
            var readingDateTime = DateTimeOffset.UtcNow;
            var value = 12345;

            var account = new Account(1, "Joe", "Bloggs", new[]
            {
                new MeterReading(Guid.NewGuid(), readingDateTime, value)
            });

            var result = account.AddMeterReading(readingDateTime.AddDays(-1), value);

            using (new AssertionScope())
            {
                result.IsSuccess.Should().BeFalse();
                result.Failure.Should().Be(Failures.AddMeterReadingToAccountFailure.NewerReadingExists);
            }
        }

        [TestCase(-100000)]
        [TestCase(100000)]
        public void MeterReadingValueValueOutOfRange(int value)
        {
            var account = new Account(1, "Joe", "Bloggs", Enumerable.Empty<MeterReading>());

            var result = account.AddMeterReading(DateTimeOffset.UtcNow, value);

            using (new AssertionScope())
            {
                result.IsSuccess.Should().BeFalse();
                result.Failure.Should().Be(Failures.AddMeterReadingToAccountFailure.ValueOutOfRange);
            }
        }

        [Test]
        public void ValidMeterReading()
        {
            var readingDateTime = DateTimeOffset.UtcNow;
            var value = 12345;

            var account = new Account(1, "Joe", "Bloggs", Enumerable.Empty<MeterReading>());

            var result = account.AddMeterReading(readingDateTime, value);

            using (new AssertionScope())
            {
                result.IsSuccess.Should().BeTrue();
                account.MeterReadings.Should().ContainSingle(meterReading =>
                    meterReading.ReadingDateTime == readingDateTime && meterReading.Value == value);
            }
        }
    }
}
