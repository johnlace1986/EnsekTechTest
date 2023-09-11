using EnsekTechTest.Services;
using Microsoft.AspNetCore.Mvc;

namespace EnsekTechTest.Controllers
{
    public class MeterReadingsController : Controller
    {
        private readonly IMeterReadingsParser _meterReadingsParser;

        public MeterReadingsController(IMeterReadingsParser meterReadingsParser)
        {
            _meterReadingsParser = meterReadingsParser;
        }

        [HttpPost]
        [Route("meter-reading-uploads")]
        public async Task<IActionResult> UploadMeterReadings([FromForm(Name = "file")] IFormFile file, CancellationToken cancellationToken)
        {
            try
            {
                using var readStream = file.OpenReadStream();
                var meterReadings = await _meterReadingsParser.ParseMeterReadings(readStream, cancellationToken);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return BadRequest();
            }

            return Ok();
        }
    }
}
