using EnsekTechTest.Persistence.DbContexts;
using EnsekTechTest.Persistence.Models;
using FluentAssertions;
using FluentAssertions.Execution;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using NUnit.Framework;
using AddMeterReadingsToAccountCommandResult = EnsekTechTest.Application.Commands.AddMeterReadingsToAccountCommand.AddMeterReadingsToAccountCommandResult;

namespace EnsekTechTest.FunctionalTests
{
    public class HappyPathTests
    {
        private static WebApplicationFactory<Program> Application => new();

        private HttpClient Client { get; set; }

        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            Client = Application.CreateClient(new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false
            });
        }

        [Test]
        public async Task UploadMeterReadings()
        {
            var successfulMeterReading = new MeterReading
            {
                AccountId = 2344,
                ReadingDateTime = ReducePrecision(DateTimeOffset.UtcNow),
                Value = 12345
            };

            using var stream = await GenerateFile(successfulMeterReading);

            using var request = new HttpRequestMessage(HttpMethod.Post, $"/meter-reading-uploads");
            using var content = new MultipartFormDataContent
            {
                { new StreamContent(stream), "file", "Meter_Reading.csv" }
            };

            request.Content = content;

            var response = await Client.SendAsync(request);

            using (new AssertionScope())
            {
                response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);

                var responseContentString = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<AddMeterReadingsToAccountCommandResult>(responseContentString);

                var context = new PersistenceContext();
                var account = context.Accounts.Include(account => account.MeterReadings).Single(a => a.Id == successfulMeterReading.AccountId);
                account.MeterReadings.Should().Contain(meterReading =>
                    ReducePrecision(meterReading.ReadingDateTime) == successfulMeterReading.ReadingDateTime &&
                    meterReading.Value == successfulMeterReading.Value);

                result.SuccessfulMeterReadings.Should().Be(1);
                result.FailedMeterReadings.Should().Be(1);
            }
        }

        private static async Task<Stream> GenerateFile(MeterReading successfulMeterReading)
        {
            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);

            await writer.WriteLineAsync("AccountId,MeterReadingDateTime,MeterReadValue");
            await writer.WriteLineAsync($"{successfulMeterReading.AccountId},{successfulMeterReading.ReadingDateTime},{successfulMeterReading.Value}");
            await writer.WriteLineAsync($"0,{DateTimeOffset.UtcNow},54321");

            await writer.FlushAsync();
            stream.Position = 0;

            return stream;
        }

        private DateTimeOffset ReducePrecision(DateTimeOffset value)
        {
            return new DateTimeOffset(value.Year, value.Month, value.Day, value.Hour, value.Minute, value.Second, value.Offset);
        }
    }
}