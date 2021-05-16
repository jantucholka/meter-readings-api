using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MeterReadings.Schema;

namespace MeterReadings.Repository
{
    public interface IMeterReadingsRepository
    {
        Task<IEnumerable<MeterReading>> AddReadings(IEnumerable<MeterReading> readings);
        Task<IEnumerable<MeterReading>> GetReadings();
        Task DeleteReading(Guid id);
        Task<MeterReading> GetReading(Guid id);
    }
}