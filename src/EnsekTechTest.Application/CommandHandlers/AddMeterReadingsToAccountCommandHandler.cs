using EnsekTechTest.Application.Commands;
using EnsekTechTest.Application.Repositories;
using MediatR;
using AddMeterReadingsToAccountCommandResult = EnsekTechTest.Application.Commands.AddMeterReadingsToAccountCommand.AddMeterReadingsToAccountCommandResult;

namespace EnsekTechTest.Application.CommandHandlers
{
    public class AddMeterReadingsToAccountCommandHandler : IRequestHandler<AddMeterReadingsToAccountCommand, AddMeterReadingsToAccountCommandResult>
    {
        private readonly IAccountsRepository _accountsRepository;

        public AddMeterReadingsToAccountCommandHandler(
            IAccountsRepository accountsRepository)
        {
            _accountsRepository = accountsRepository;
        }

        public async Task<AddMeterReadingsToAccountCommandResult> Handle(AddMeterReadingsToAccountCommand command, CancellationToken cancellationToken)
        {
            var result = new AddMeterReadingsToAccountCommandResult();

            var account = await _accountsRepository.GetByIdAsync(command.AccountId, cancellationToken);

            if (account is null)
            {
                result.FailedMeterReadings = command.MeterReadings.Count();
                return result;
            }

            foreach (var meterReading in command.MeterReadings)
            {
                var addMeterReadingResult = account.AddMeterReading(meterReading.ReadingDateTime, meterReading.Value);

                if (addMeterReadingResult.IsSuccess)
                    result.SuccessfulMeterReadings++;
                else
                    result.FailedMeterReadings++;
            }

            if (result.SuccessfulMeterReadings > 0)
                await _accountsRepository.CommitChanges(account, cancellationToken);

            return result;
        }
    }
}
