namespace MeterReadings.Schema
{
    public class MeterReadingLenient
    {
        public override string ToString()
        {
            return $"{nameof(AccountId)}: {AccountId}, {nameof(MeterReadingDateTime)}: {MeterReadingDateTime}, {nameof(MeterReadValue)}: {MeterReadValue}";
        }

        public string AccountId { get; set; }
        public string MeterReadingDateTime { get; set; }
        public string MeterReadValue { get; set; }

        public string Errors { get; set; }
    }
}