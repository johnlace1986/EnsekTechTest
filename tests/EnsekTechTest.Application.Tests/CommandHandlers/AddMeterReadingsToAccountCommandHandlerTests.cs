using EnsekTechTest.Application.CommandHandlers;
using EnsekTechTest.Application.Commands;
using EnsekTechTest.Application.Infrastructure;
using EnsekTechTest.Application.Infrastructure.Repositories;
using FluentAssertions;
using FluentAssertions.Execution;
using Moq;
using NUnit.Framework;
using AddMeterReadingsToAccountCommandResult = EnsekTechTest.Application.Commands.AddMeterReadingsToAccountCommand.AddMeterReadingsToAccountCommandResult;

namespace EnsekTechTest.Application.Tests.CommandHandlers
{
    public class AddMeterReadingsToAccountCommandHandlerTests
    {
        [Test]
        public async Task AccountNotFound()
        {
            var accountRepositoryMock = new Mock<IAccountsRepository>();
            accountRepositoryMock.Setup(mock => mock.GetByIdAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult<Domain.AggregateRoots.Account>(null));

            var unitOfWorkMock = new Mock<IUnitOfWork>();

            var sut = new AddMeterReadingsToAccountCommandHandler(
                accountRepositoryMock.Object,
                Mock.Of<IMeterReadingsRepository>(),
                unitOfWorkMock.Object);

            var command = new AddMeterReadingsToAccountCommand
            {
                AccountId = 1,
                MeterReadings = new[]
                {
                    new AddMeterReadingsToAccountCommand.MeterReading(),
                    new AddMeterReadingsToAccountCommand.MeterReading()
                }
            };

            var result = await sut.Handle(command, CancellationToken.None);

            using (new AssertionScope())
            {
                result.Should().BeEquivalentTo(new AddMeterReadingsToAccountCommandResult
                {
                    SuccessfulMeterReadings = 0,
                    FailedMeterReadings = 2
                });

                unitOfWorkMock.Verify(mock => mock.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
            }
        }

        [Test]
        public async Task NoSuccessfulReadings()
        {
            var account = new Domain.AggregateRoots.Account(1, "Joe", "Bloggs", Enumerable.Empty<Domain.Entities.MeterReading>());

            var accountRepositoryMock = new Mock<IAccountsRepository>();
            accountRepositoryMock.Setup(mock => mock.GetByIdAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(account);

            var meterReadingsRepositoryMock = new Mock<IMeterReadingsRepository>();
            var unitOfWorkMock = new Mock<IUnitOfWork>();

            var sut = new AddMeterReadingsToAccountCommandHandler(
                accountRepositoryMock.Object,
                meterReadingsRepositoryMock.Object,
                unitOfWorkMock.Object);

            var command = new AddMeterReadingsToAccountCommand
            {
                AccountId = 1,
                MeterReadings = new[]
                {
                    new AddMeterReadingsToAccountCommand.MeterReading{ ReadingDateTime = DateTimeOffset.UtcNow, Value = 100000},
                }
            };

            await sut.Handle(command, CancellationToken.None);

            unitOfWorkMock.Verify(mock => mock.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
        }

        [Test]
        public async Task ValidCommand()
        {
            var account = new Domain.AggregateRoots.Account(1, "Joe", "Bloggs", Enumerable.Empty<Domain.Entities.MeterReading>());

            var accountRepositoryMock = new Mock<IAccountsRepository>();
            accountRepositoryMock.Setup(mock => mock.GetByIdAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(account);

            var meterReadingsRepositoryMock = new Mock<IMeterReadingsRepository>();
            var unitOfWorkMock = new Mock<IUnitOfWork>();

            var sut = new AddMeterReadingsToAccountCommandHandler(
                accountRepositoryMock.Object, 
                meterReadingsRepositoryMock.Object,
                unitOfWorkMock.Object);

            var now = DateTimeOffset.UtcNow;

            var repeatingMeterReading = new AddMeterReadingsToAccountCommand.MeterReading
            {
                ReadingDateTime = now.AddDays(1),
                Value = 23456
            };

            var command = new AddMeterReadingsToAccountCommand
            {
                AccountId = 1,
                MeterReadings = new[]
                {
                    new AddMeterReadingsToAccountCommand.MeterReading{ ReadingDateTime = now, Value = 12345},
                    repeatingMeterReading,
                    repeatingMeterReading
                }
            };

            var result = await sut.Handle(command, CancellationToken.None);

            using (new AssertionScope())
            {
                meterReadingsRepositoryMock.Verify(
                    mock => mock.AddMeterReadingAsync(1, now, 12345, It.IsAny<CancellationToken>()), Times.Once);

                meterReadingsRepositoryMock.Verify(
                    mock => mock.AddMeterReadingAsync(1, now.AddDays(1), 23456, It.IsAny<CancellationToken>()), Times.Once);

                unitOfWorkMock.Verify(mock => mock.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);

                result.Should().BeEquivalentTo(new AddMeterReadingsToAccountCommandResult
                {
                    SuccessfulMeterReadings = 2,
                    FailedMeterReadings = 1
                });
            }
        }
    }
}
