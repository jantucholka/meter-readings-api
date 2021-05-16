using FluentValidation.Results;
using MeterReadings.Schema;

namespace MeterReading.Logic.Validators
{
    public interface IMeterReadingLenientValidator
    {
        ValidationResult Validate(MeterReadingLenient instance);
    }
}