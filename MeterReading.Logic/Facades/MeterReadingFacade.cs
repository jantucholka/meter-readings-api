using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using MeterReading.Logic.Validators;
using MeterReadings.Repository;
using MeterReadings.Schema;

namespace MeterReading.Logic.Facades
{
    public class MeterReadingFacade : IMeterReadingFacade
    {
        public MeterReadingFacade(
            IMeterReadingsRepository meterReadingsRepository,
            IMeterReadingLenientValidator meterReadingLenientValidator)
        {
            _meterReadingsRepository = meterReadingsRepository ??
                                            throw new ArgumentNullException(nameof(meterReadingsRepository));
            _meterReadingLenientValidator = meterReadingLenientValidator ??
                                            throw new ArgumentNullException(nameof(meterReadingLenientValidator));
        }

        private readonly IMeterReadingsRepository _meterReadingsRepository;
        private readonly IMeterReadingLenientValidator _meterReadingLenientValidator;

        public async Task<AddMeterStatusResponse> AddMeterReadings(Collection<HttpContent> contents)
        {
            // read csv
            var readings = await CsvHelper.ReadCsvFromRequestIntoCollectionOfType<MeterReadingLenient>(contents);

            var enumeratedMeterReadings = readings.ToList();

            // run validator
            foreach (var reading in enumeratedMeterReadings)
            {
                var errors = string.Join(", ", _meterReadingLenientValidator.Validate(reading).Errors.Select(error => error.ErrorMessage));
                reading.Errors = !string.IsNullOrWhiteSpace(errors) ? errors : null;
            }

            // map to internal schema
            var readingsToAdd = new List<MeterReadings.Schema.MeterReading>();
            readingsToAdd.AddRange(enumeratedMeterReadings
                .Where(reading => string.IsNullOrWhiteSpace(reading.Errors))
                .Select(reading => reading.ToMeterReading()));

            // call repo with readings that satisfy data contract validation
            var successfullyAddedReadings = await _meterReadingsRepository.AddReadings(readingsToAdd);

            var failuresDueToDataContractValidation = enumeratedMeterReadings.Where(reading => !string.IsNullOrWhiteSpace(reading.Errors));
            var numberOfSuccessfullyAddedReading = successfullyAddedReadings.Count();

            var errorMessages = new List<string>();

            errorMessages.AddRange(failuresDueToDataContractValidation.Select(reading => $"{reading} - {reading.Errors}"));

            var readingsRejectedByTheDatabase = readingsToAdd
                .Where(readingToAdd => successfullyAddedReadings
                    .All(
                        addedReading =>
                            !(readingToAdd.AccountId == addedReading.AccountId &&
                            readingToAdd.MeterReadingDateTime == addedReading.MeterReadingDateTime &&
                            readingToAdd.MeterReadValue == addedReading.MeterReadValue)
                        ));

            errorMessages.AddRange(readingsRejectedByTheDatabase.Select(failedToAddReading => $"{failedToAddReading} - AccountId doesn't exist or reading has already been added"));

            return new AddMeterStatusResponse()
            {
                SuccesfullCount = numberOfSuccessfullyAddedReading,
                FailedCount = enumeratedMeterReadings.Count - numberOfSuccessfullyAddedReading,
                Errors = errorMessages
            };
        }

        public async Task<Guid?> AddMeterReading(MeterReadings.Schema.MeterReading meterReading)
        {
            var successfullyAddedReadings = await _meterReadingsRepository.AddReadings(new[] { meterReading });

            return successfullyAddedReadings.FirstOrDefault()?.Id;
        }

        public Task<IEnumerable<MeterReadings.Schema.MeterReading>> GetReadings() => _meterReadingsRepository.GetReadings();
        public Task DeleteReading(Guid id) => _meterReadingsRepository.DeleteReading(id);
        public Task<MeterReadings.Schema.MeterReading> GetReading(Guid id) => _meterReadingsRepository.GetReading(id);
    }
}
