using EnsekTechTest.Application.Commands;
using EnsekTechTest.Core;
using MediatR;
using Unit = EnsekTechTest.Core.Unit;

namespace EnsekTechTest.Application.CommandHandlers
{
    public class AddMeterReadingsToAccountCommandHandler : IRequestHandler<AddMeterReadingsToAccountCommand, Result<Unit, string>>
    {
        public Task<Result<Unit, string>> Handle(AddMeterReadingsToAccountCommand request, CancellationToken cancellationToken)
        {
            return Task.FromResult(new Result<Unit, string>(Unit.Instance));
        }
    }
}
