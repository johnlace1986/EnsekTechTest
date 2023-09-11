using EnsekTechTest.Services;
using FluentAssertions;
using FluentAssertions.Execution;
using NUnit.Framework;
using System.Text;

namespace EnsekTechTest.Tests.Services
{
    public class MeterReadingsParserTests
    {
        [Test]
        public async Task ValidData()
        {
            var now = DateTimeOffset.UtcNow;

            var expectedMeterReading = new MeterReading
            {
                AccountId = 1234,
                ReadingDateTime = now,
                Value = 23456
            };

            var builder = new StringBuilder();
            builder.AppendLine("AccountId,MeterReadingDateTime,MeterReadValue");
            builder.AppendLine($"{expectedMeterReading.AccountId},{expectedMeterReading.ReadingDateTime},{expectedMeterReading.Value}");

            using var stream = new MemoryStream(Encoding.Default.GetBytes(builder.ToString()));

            var sut = new MeterReadingsParser();

            var result = await sut.ParseMeterReadings(stream, CancellationToken.None);

            using (new AssertionScope())
            {
                var parsedMeterReading = result.Should().ContainSingle().Subject;
                parsedMeterReading.AccountId.Should().Be(expectedMeterReading.AccountId);
                parsedMeterReading.ReadingDateTime.Should().BeCloseTo(now, TimeSpan.FromSeconds(1));
                parsedMeterReading.Value.Should().Be(expectedMeterReading.Value);
            }
        }
    }
}
