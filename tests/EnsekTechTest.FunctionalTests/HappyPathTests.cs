using FluentAssertions;
using FluentAssertions.Execution;
using Microsoft.AspNetCore.Mvc.Testing;
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
            using var stream = await GenerateFile();

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

                result.SuccessfulMeterReadings.Should().Be(1);
                result.FailedMeterReadings.Should().Be(1);
            }
        }

        private static async Task<Stream> GenerateFile()
        {
            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);

            await writer.WriteLineAsync("AccountId,MeterReadingDateTime,MeterReadValue");
            await writer.WriteLineAsync($"2344,{DateTimeOffset.UtcNow},12345");
            await writer.WriteLineAsync($"0,{DateTimeOffset.UtcNow},54321");

            await writer.FlushAsync();
            stream.Position = 0;

            return stream;
        }
    }
}