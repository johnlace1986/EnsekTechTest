using EnsekTechTest.Application.Commands;
using EnsekTechTest.Controllers;
using EnsekTechTest.Services;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using AddMeterReadingsToAccountCommandResult = EnsekTechTest.Application.Commands.AddMeterReadingsToAccountCommand.AddMeterReadingsToAccountCommandResult;

namespace EnsekTechTest.Tests.Controllers
{
    public class MeterReadingsControllerTests
    {
        [Test]
        public async Task MalformedCsvFile()
        {
            var meterReadingsParserMock = new Mock<IMeterReadingsParser>();
            meterReadingsParserMock.Setup(mock => mock.ParseMeterReadings(It.IsAny<Stream>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new Exception());

            var sut = new MeterReadingsController(
                meterReadingsParserMock.Object,
                Mock.Of<IMediator>());

            var result = await sut.UploadMeterReadingsAsync(FormFile, CancellationToken.None);

            result.Should().BeOfType<BadRequestResult>();
        }

        [Test]
        public async Task CommandSentForEachReading()
        {
            var meterReadings = new[]
            {
                new IMeterReadingsParser.MeterReading { AccountId = 1, ReadingDateTime = DateTimeOffset.UtcNow, Value = 12345 },
                new IMeterReadingsParser.MeterReading { AccountId = 1, ReadingDateTime = DateTimeOffset.UtcNow.AddDays(5), Value = 23456 },
                new IMeterReadingsParser.MeterReading { AccountId = 99, ReadingDateTime = DateTimeOffset.UtcNow.AddDays(10), Value = 54321 }
            };

            var meterReadingsParserMock = new Mock<IMeterReadingsParser>();
            meterReadingsParserMock.Setup(mock => mock.ParseMeterReadings(It.IsAny<Stream>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(meterReadings);

            var mediatorMock = new Mock<IMediator>();
            mediatorMock.Setup(mock => mock.Send(It.IsAny<AddMeterReadingsToAccountCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new AddMeterReadingsToAccountCommandResult());

            var sut = new MeterReadingsController(
                meterReadingsParserMock.Object,
                mediatorMock.Object);

            await sut.UploadMeterReadingsAsync(FormFile, CancellationToken.None);

            mediatorMock.Verify(mock => mock.Send(It.IsAny<AddMeterReadingsToAccountCommand>(), It.IsAny<CancellationToken>()), Times.Exactly(2));
        }

        [Test]
        public async Task ValidCsvFile()
        {
            var meterReadings = new[]
            {
                new IMeterReadingsParser.MeterReading { AccountId = 1, ReadingDateTime = DateTimeOffset.UtcNow, Value = 12345 },
                new IMeterReadingsParser.MeterReading { AccountId = 99, ReadingDateTime = DateTimeOffset.UtcNow.AddDays(10), Value = 54321 }
            };

            var meterReadingsParserMock = new Mock<IMeterReadingsParser>();
            meterReadingsParserMock.Setup(mock => mock.ParseMeterReadings(It.IsAny<Stream>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(meterReadings);

            var mediatorMock = new Mock<IMediator>();
            mediatorMock.SetupSequence(mock => mock.Send(It.IsAny<AddMeterReadingsToAccountCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new AddMeterReadingsToAccountCommandResult
                {
                    SuccessfulMeterReadings = 1,
                    FailedMeterReadings = 2
                })
                .ReturnsAsync(new AddMeterReadingsToAccountCommandResult
                {
                    SuccessfulMeterReadings = 0,
                    FailedMeterReadings = 100
                });

            var sut = new MeterReadingsController(
                meterReadingsParserMock.Object,
                mediatorMock.Object);

            var result = await sut.UploadMeterReadingsAsync(FormFile, CancellationToken.None);

            result.Should().BeOfType<OkObjectResult>()
                .Which.Value.Should().BeOfType<AddMeterReadingsToAccountCommandResult>()
                .Which.Should().BeEquivalentTo(new AddMeterReadingsToAccountCommandResult
                {
                    SuccessfulMeterReadings = 1,
                    FailedMeterReadings = 102
                });
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
