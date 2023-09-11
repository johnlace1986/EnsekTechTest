using EnsekTechTest.Core;
using MediatR;
using Unit = EnsekTechTest.Core.Unit;

namespace EnsekTechTest.Application.Commands
{
    public class AddMeterReadingsToAccountCommand : IRequest<Result<Unit, string>>
    {
        public int AccountId { get; set; }

        public IEnumerable<MeterReading> MeterReadings { get; set; }

        public class MeterReading
        {
            public DateTimeOffset ReadingDateTime { get; set; }

            public int Value { get; set; }
        }
    }
}
