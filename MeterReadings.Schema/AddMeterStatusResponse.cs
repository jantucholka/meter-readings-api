using System.Collections.Generic;

namespace MeterReadings.Schema
{
    public class AddMeterStatusResponse
    {
        public int SuccesfullCount { get; set; }
        public int FailedCount { get; set; }

        public IEnumerable<string> Errors { get; set; }
    }
}