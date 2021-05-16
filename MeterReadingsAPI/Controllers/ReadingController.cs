using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Results;
using MeterReading.Logic;

namespace MeterReadingsAPI.Controllers
{
    public class ReadingController : ApiController
    {
        private readonly IMeterReadingFacade _meterReadingFacade;

        public ReadingController(IMeterReadingFacade meterReadingFacade)
        {
            _meterReadingFacade = meterReadingFacade ?? throw new ArgumentNullException(nameof(meterReadingFacade));
        }

        // GET: api/Reading
        public async Task<IEnumerable<MeterReadings.Schema.MeterReading>> Get()
        {
            return await _meterReadingFacade.GetReadings();
        }

        // GET: api/Reading/5
        public async Task<IHttpActionResult> Get(Guid id)
        {
            var reading = await _meterReadingFacade.GetReading(id);

            if (reading != null)
            {
                return new NegotiatedContentResult<MeterReadings.Schema.MeterReading>(HttpStatusCode.OK, reading, this);
            }

            return NotFound();
        }

        // POST: api/Reading
        public async Task<IHttpActionResult> Post(MeterReadings.Schema.MeterReading meterReading)
        {
            var readingId = await _meterReadingFacade.AddMeterReading(meterReading);

            if (readingId.HasValue)
            {
                return new NegotiatedContentResult<Guid>(HttpStatusCode.Created,
                    readingId.Value, this);
            }

            return BadRequest();
        }

        // DELETE: api/Reading/5
        public async Task<HttpStatusCode> Delete(Guid id)
        {
            await _meterReadingFacade.DeleteReading(id);

            return HttpStatusCode.NoContent;
        }
    }
}
