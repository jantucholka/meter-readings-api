using System;
using System.Globalization;
using System.Text.RegularExpressions;
using FluentValidation;
using MeterReadings.Schema;

namespace MeterReading.Logic.Validators
{
    public class MeterReadingLenientValidator : AbstractValidator<MeterReadingLenient>, IMeterReadingLenientValidator
    {
        private string[] validDateTimeFormats = new[] {"dd/MM/yyyy hh:mm", "dd/MM/yyyy hh:mm:ss", "dd/MM/yyyy  hh:mm", "dd/MM/yyyy  hh:mm:ss" };

        public MeterReadingLenientValidator()
        {
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
                    if (!DateTime.TryParseExact(s, validDateTimeFormats, CultureInfo.InvariantCulture,
                        DateTimeStyles.None,
                        out DateTime result))
                    {
                        context.AddFailure($"{nameof(MeterReadingLenient.MeterReadingDateTime)} must be in one of the following formats: {string.Join(", ", validDateTimeFormats)}");
                    }
                });
            RuleFor(reading => reading.MeterReadValue)
                .NotNull()
                .Custom((s, context) =>
                {
                    if (string.IsNullOrWhiteSpace(s) || !Regex.IsMatch(s, @"[0-9]{5}"))
                        context.AddFailure($"{nameof(MeterReadingLenient.MeterReadValue)} must consist of five digits. Value provided: {s}");
                });
        }
    }
}