using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Threading.Tasks;

namespace MeterReading.Logic
{
    public interface ICsvHelper
    {
        Task<IEnumerable<T>> ReadCsvFromRequestIntoCollectionOfType<T>(Collection<HttpContent> contents);
    }
}