using EnsekTechTest.Application.Commands;
using EnsekTechTest.Application.Infrastructure;
using EnsekTechTest.Application.Infrastructure.Repositories;
using MediatR;
using AddMeterReadingsToAccountCommandResult = EnsekTechTest.Application.Commands.AddMeterReadingsToAccountCommand.AddMeterReadingsToAccountCommandResult;

namespace EnsekTechTest.Application.CommandHandlers
{
    public class AddMeterReadingsToAccountCommandHandler : IRequestHandler<AddMeterReadingsToAccountCommand, AddMeterReadingsToAccountCommandResult>
    {
        private readonly IAccountsRepository _accountsRepository;
        private readonly IMeterReadingsRepository _meterReadingsRepository;
        private readonly IUnitOfWork _unitOfWork;

        public AddMeterReadingsToAccountCommandHandler(
            IAccountsRepository accountsRepository,
            IMeterReadingsRepository meterReadingsRepository,
            IUnitOfWork unitOfWork)
        {
            _accountsRepository = accountsRepository;
            _meterReadingsRepository = meterReadingsRepository;
            _unitOfWork = unitOfWork;
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
                {
                    result.SuccessfulMeterReadings++;
                    await _meterReadingsRepository.AddMeterReadingAsync(account.Id, meterReading.ReadingDateTime, meterReading.Value, cancellationToken);
                }
                else
                    result.FailedMeterReadings++;
            }

            if (result.SuccessfulMeterReadings > 0)
                await _unitOfWork.SaveChangesAsync(cancellationToken);

            return result;
        }
    }
}
