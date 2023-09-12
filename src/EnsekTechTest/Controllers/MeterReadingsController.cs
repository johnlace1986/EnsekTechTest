using EnsekTechTest.Application.Commands;
using EnsekTechTest.Core;
using EnsekTechTest.Services;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using AddMeterReadingsToAccountCommandResult = EnsekTechTest.Application.Commands.AddMeterReadingsToAccountCommand.AddMeterReadingsToAccountCommandResult;

namespace EnsekTechTest.Controllers
{
    public class MeterReadingsController : Controller
    {
        private readonly IMeterReadingsParser _meterReadingsParser;
        private readonly IMediator _mediator;

        public MeterReadingsController(
            IMeterReadingsParser meterReadingsParser,
            IMediator mediator)
        {
            _meterReadingsParser = meterReadingsParser;
            _mediator = mediator;
        }

        [HttpPost]
        [Route("meter-reading-uploads")]
        public async Task<IActionResult> UploadMeterReadingsAsync([FromForm(Name = "file")] IFormFile file, CancellationToken cancellationToken)
        {
            try
            {
                IEnumerable<IMeterReadingsParser.MeterReading> meterReadings;

                try
                {
                    using var readStream = file.OpenReadStream();
                    meterReadings = await _meterReadingsParser.ParseMeterReadings(readStream, cancellationToken);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return BadRequest();
                }

                var commands = GroupMeterReadingsByAccount(meterReadings);

                var results = await Task.WhenAll(commands.Select(command =>
                {
                    return _mediator.Send(command, cancellationToken);
                }));

                var overallResult = new AddMeterReadingsToAccountCommandResult
                {
                    SuccessfulMeterReadings = results.Sum(result => result.SuccessfulMeterReadings),
                    FailedMeterReadings = results.Sum(result => result.FailedMeterReadings)
                };

                return Ok(overallResult);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        private static IEnumerable<AddMeterReadingsToAccountCommand> GroupMeterReadingsByAccount(IEnumerable<IMeterReadingsParser.MeterReading> meterReadings)
        {
            var commands =
                from meterReading in meterReadings
                group meterReading by meterReading.AccountId into accounts
                select new AddMeterReadingsToAccountCommand
                {
                    AccountId = accounts.Key,
                    MeterReadings = accounts
                        .Select(groupedMeterReading => new AddMeterReadingsToAccountCommand.MeterReading
                        {
                            ReadingDateTime = groupedMeterReading.ReadingDateTime,
                            Value = groupedMeterReading.Value
                        })
                        .OrderBy(groupedMeterReading => groupedMeterReading.ReadingDateTime)
                    
                };

            return commands;
        }
    }
}
