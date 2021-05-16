using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Results;
using MeterReading.Logic;
using MeterReadings.Schema;

namespace MeterReadingsAPI.Controllers
{
    public class MeterReadingUploadsController : ApiController
    {
        private readonly IMeterReadingFacade _meterReadingFacade;

        public MeterReadingUploadsController(IMeterReadingFacade meterReadingFacade)
        {
            _meterReadingFacade = meterReadingFacade ?? throw new ArgumentNullException(nameof(meterReadingFacade));
        }

        public async Task<IHttpActionResult> Post()
        {
            if (!Request.Content.IsMimeMultipartContent())
                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);

            var provider = new MultipartMemoryStreamProvider();
            await Request.Content.ReadAsMultipartAsync(provider);

            var response = await _meterReadingFacade.AddMeterReadings(provider.Contents);

            return new NegotiatedContentResult<AddMeterStatusResponse>(HttpStatusCode.OK,
                response, this);
        }
    }
}
