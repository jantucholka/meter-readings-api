using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Threading.Tasks;
using MeterReadings.Schema;

namespace MeterReading.Logic.Facades
{
    public interface IMeterReadingFacade
    {
        Task<AddMeterStatusResponse> AddMeterReadings(Collection<HttpContent> contents);
        Task<Guid?> AddMeterReading(MeterReadings.Schema.MeterReading meterReading);
        Task<IEnumerable<MeterReadings.Schema.MeterReading>> GetReadings();
        Task DeleteReading(Guid id);
        Task<MeterReadings.Schema.MeterReading> GetReading(Guid id);
    }
}