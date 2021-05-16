using System.Collections.Generic;
using System.Threading.Tasks;
using MeterReadings.Schema;

namespace MeterReadings.Repository
{
    public interface IMeterReadingsRepository
    {
        Task<IEnumerable<MeterReading>> AddReadings(IEnumerable<MeterReading> readings);
        Task<IEnumerable<MeterReading>> GetReadings(IEnumerable<MeterReading> readings);
    }
}