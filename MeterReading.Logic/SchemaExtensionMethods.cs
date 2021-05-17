using System;
using System.Globalization;

namespace MeterReading.Logic
{
    public static class SchemaExtensionMethods
    {
        public static MeterReadings.Schema.MeterReading ToMeterReading(this MeterReadings.Schema.MeterReadingLenient source)
        {
            var tmp = new MeterReadings.Schema.MeterReading()
            {
                AccountId = int.TryParse(source.AccountId, out int accountId) ? accountId : throw new ArgumentException($"{source.AccountId} is not an integer"),
                MeterReadValue = source.MeterReadValue,
                MeterReadingDateTime = DateTime.TryParseExact(source.MeterReadingDateTime, "dd/MM/yyyy hh:mm", new DateTimeFormatInfo(),
                    DateTimeStyles.AssumeLocal,
                    out DateTime meterReadingDateTime) ? meterReadingDateTime : throw new ArgumentException($"{source.AccountId} is not a datetime"),
            };

            return tmp;
        }
    }
}
