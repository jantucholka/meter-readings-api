using System.Collections.ObjectModel;
using System.Net.Http;
using System.Threading.Tasks;
using MeterReadings.Schema;

namespace MeterReading.Logic
{
    public interface IMeterReadingFacade
    {
        Task<AddMeterStatusResponse> AddMeterReadings(Collection<HttpContent> contents);
    }
}