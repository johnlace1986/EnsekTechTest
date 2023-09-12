﻿using EnsekTechTest.Application.CommandHandlers;
using EnsekTechTest.Application.Commands;
using EnsekTechTest.Application.Repositories;
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

            var sut = new AddMeterReadingsToAccountCommandHandler(accountRepositoryMock.Object);

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
            result.Should().BeEquivalentTo(new AddMeterReadingsToAccountCommandResult
            {
                SuccessfulMeterReadings = 0,
                FailedMeterReadings = 2
            });
        }

        [Test]
        public async Task ValidCommand()
        {
            var account = new Domain.AggregateRoots.Account(1, "Joe", "Bloggs", Enumerable.Empty<Domain.Entities.MeterReading>());

            var accountRepositoryMock = new Mock<IAccountsRepository>();
            accountRepositoryMock.Setup(mock => mock.GetByIdAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(account);

            var sut = new AddMeterReadingsToAccountCommandHandler(accountRepositoryMock.Object);

            var repeatingMeterReading = new AddMeterReadingsToAccountCommand.MeterReading
            {
                ReadingDateTime = DateTimeOffset.UtcNow.AddDays(1),
                Value = 23456
            };

            var command = new AddMeterReadingsToAccountCommand
            {
                AccountId = 1,
                MeterReadings = new[]
                {
                    new AddMeterReadingsToAccountCommand.MeterReading{ ReadingDateTime = DateTimeOffset.UtcNow, Value = 12345},
                    repeatingMeterReading,
                    repeatingMeterReading
                }
            };

            var result = await sut.Handle(command, CancellationToken.None);

            using (new AssertionScope())
            {
                result.Should().BeEquivalentTo(new AddMeterReadingsToAccountCommandResult
                {
                    SuccessfulMeterReadings = 2,
                    FailedMeterReadings = 1
                });

                accountRepositoryMock.Verify(mock => mock.CommitChanges(account, It.IsAny<CancellationToken>()), Times.Once());
            }
        }
    }
}
