using System;
using System.Globalization;
using System.Text.RegularExpressions;
using FluentValidation;
using MeterReadings.Schema;

namespace MeterReading.Logic.Validators
{
    public class MeterReadingLenientValidator : AbstractValidator<MeterReadingLenient>, IMeterReadingLenientValidator
    {
        public MeterReadingLenientValidator()
        {
            // TODO prevent the same entry from being loaded twice

            RuleFor(reading => reading.AccountId)
                .NotNull()
                .Custom((s, context) =>
                {
                    if (!int.TryParse(s, out int result))
                        context.AddFailure($"{nameof(MeterReadingLenient.AccountId)} must be a valid integer. Value provided: {s}");
                });
            RuleFor(reading => reading.MeterReadingDateTime)
                .NotNull()
                .Custom((s, context) =>
                {
                    if (!DateTime.TryParseExact(s, "dd/MM/yyyy hh:mm", new DateTimeFormatInfo(),
                        DateTimeStyles.AssumeLocal,
                        out DateTime result))
                    {
                        context.AddFailure($"{nameof(MeterReadingLenient.MeterReadingDateTime)} must be in the following format: 'dd/MM/yyyy hh:mm'");
                    }
                });
            RuleFor(reading => reading.MeterReadValue)
                .NotNull()
                .Custom((s, context) =>
                {
                    if (!Regex.IsMatch(s, @"[0-9]{5}"))
                        context.AddFailure($"{nameof(MeterReadingLenient.MeterReadValue)} must consist of five digits. Value provided: {s}");
                });
        }
    }
}