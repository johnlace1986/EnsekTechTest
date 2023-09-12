using EnsekTechTest.Application.Commands;
using EnsekTechTest.Application.Failures;
using EnsekTechTest.Application.Repositories;
using EnsekTechTest.Core;
using MediatR;
using AddMeterReadingsToAccountCommandResult = EnsekTechTest.Application.Commands.AddMeterReadingsToAccountCommand.AddMeterReadingsToAccountCommandResult;

namespace EnsekTechTest.Application.CommandHandlers
{
    public class AddMeterReadingsToAccountCommandHandler : IRequestHandler<AddMeterReadingsToAccountCommand, Result<AddMeterReadingsToAccountCommandResult, AddMeterReadingsToAccountFailure>>
    {
        private readonly IAccountsRepository _accountsRepository;

        public AddMeterReadingsToAccountCommandHandler(
            IAccountsRepository accountsRepository)
        {
            _accountsRepository = accountsRepository;
        }

        public async Task<Result<AddMeterReadingsToAccountCommandResult, AddMeterReadingsToAccountFailure>> Handle(AddMeterReadingsToAccountCommand command, CancellationToken cancellationToken)
        {
            var account = await _accountsRepository.GetByIdAsync(command.AccountId, cancellationToken);

            if (account is null)
                return AddMeterReadingsToAccountFailure.AccountNotFound;

            var result = new AddMeterReadingsToAccountCommandResult();

            foreach (var meterReading in command.MeterReadings)
            {
                var addMeterReadingResult = account.AddMeterReading(meterReading.ReadingDateTime, meterReading.Value);

                if (addMeterReadingResult.IsSuccess)
                    result.SuccessfulMeterReadings++;
                else
                    result.FailedMeterReadings++;
            }

            await _accountsRepository.CommitChanges(account, cancellationToken);

            return new Result<AddMeterReadingsToAccountCommandResult, AddMeterReadingsToAccountFailure>(result);
        }
    }
}
