using EnsekTechTest.Application.Failures;
using EnsekTechTest.Core;
using MediatR;
using static EnsekTechTest.Application.Commands.AddMeterReadingsToAccountCommand;

namespace EnsekTechTest.Application.Commands
{
    public class AddMeterReadingsToAccountCommand : IRequest<Result<AddMeterReadingsToAccountCommandResult, AddMeterReadingsToAccountFailure>>
    {
        public int AccountId { get; set; }

        public IEnumerable<MeterReading> MeterReadings { get; set; }

        public class MeterReading
        {
            public DateTimeOffset ReadingDateTime { get; set; }

            public int Value { get; set; }
        }

        public class AddMeterReadingsToAccountCommandResult
        {
            public int SuccessfulMeterReadings { get; set; }

            public int FailedMeterReadings { get; set; }
        }
    }
}
