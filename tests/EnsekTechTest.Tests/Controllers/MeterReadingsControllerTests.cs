using EnsekTechTest.Controllers;
using EnsekTechTest.Services;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;

namespace EnsekTechTest.Tests.Controllers
{
    public class MeterReadingsControllerTests
    {
        [Test]
        public async Task MalformedCsvFile()
        {
            var meterReadinsParserMock = new Mock<IMeterReadingsParser>();
            meterReadinsParserMock.Setup(mock => mock.ParseMeterReadings(It.IsAny<Stream>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new Exception());

            var sut = new MeterReadingsController(
                meterReadinsParserMock.Object);

            var result = await sut.UploadMeterReadings(FormFile, CancellationToken.None);

            result.Should().BeOfType<BadRequestResult>();
        }

        [Test]
        public async Task ValidCsvFile()
        {
            var meterReadinsParserMock = new Mock<IMeterReadingsParser>();
            meterReadinsParserMock.Setup(mock => mock.ParseMeterReadings(It.IsAny<Stream>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(Enumerable.Empty<MeterReading>());

            var sut = new MeterReadingsController(
                meterReadinsParserMock.Object);

            var result = await sut.UploadMeterReadings(FormFile, CancellationToken.None);

            result.Should().BeOfType<OkResult>();
        }

        private static IFormFile FormFile
        {
            get
            {
                var formFileMock = new Mock<IFormFile>();
                formFileMock.Setup(mock => mock.OpenReadStream()).Returns(new MemoryStream());

                return formFileMock.Object;
            }
        }
    }
}
