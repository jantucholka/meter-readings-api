using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using CsvHelper;
using CsvHelper.Configuration;

namespace MeterReading.Logic
{
    public class CsvHelper
    {
        public static async Task<IEnumerable<T>> ReadCsvFromRequestIntoCollectionOfType<T>(Collection<HttpContent> contents)
        {
            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                MissingFieldFound = null,
                HeaderValidated = null,
            };

            var readings = new List<T>();

            foreach (var file in contents)
            {
                var filename = file.Headers.ContentDisposition.FileName.Trim('\"');
                var buffer = await file.ReadAsStringAsync();
                using (var reader = new StreamReader(await file.ReadAsStreamAsync()))
                {
                    using (var csv = new CsvReader(reader, config))
                    {
                        readings.AddRange(csv.GetRecords<T>());
                    }
                }
            }

            return readings;
        }
    }
}