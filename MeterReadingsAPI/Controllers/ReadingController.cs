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
    public class ReadingController : ApiController
    {
        private readonly IMeterReadingFacade _meterReadingFacade;

        public ReadingController(IMeterReadingFacade meterReadingFacade)
        {
            _meterReadingFacade = meterReadingFacade ?? throw new ArgumentNullException(nameof(meterReadingFacade));
        }

        // GET: api/Reading
        public IEnumerable<string> Get()
        {
            return new [] { "value1", "value2" };
        }

        // GET: api/Reading/5
        public string Get(int id)
        {
            return "value";
        }

        //// POST: api/Reading
        //public void Post([FromBody]string value)
        //{
        //}

        // POST: api/Reading


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

        // PUT: api/Reading/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Reading/5
        public void Delete(int id)
        {
        }
    }
}
