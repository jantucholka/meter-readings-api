using System;

namespace MeterReadings.Schema
{
    public class MeterReading
    {
        public int AccountId { get; set; }
        public DateTime MeterReadingDateTime { get; set; }
        public string MeterReadValue { get; set; }
        public Guid Id { get; set; }

        public override string ToString()
        {
            return $"{nameof(AccountId)}: {AccountId}, {nameof(MeterReadingDateTime)}: {MeterReadingDateTime}, {nameof(MeterReadValue)}: {MeterReadValue}";
        }
    }
}