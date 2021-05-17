using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using MeterReadings.Schema;

namespace MeterReadings.Repository
{
    public class MeterReadingsRepository : IMeterReadingsRepository
    {
        private readonly string connectionString = ConfigurationManager.ConnectionStrings["meterReadings"]
            .ConnectionString;

        public async Task<IEnumerable<MeterReading>> AddReadings(IEnumerable<MeterReading> readings)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            using (var command = connection.CreateCommand())
            {
                using (var table = new DataTable())
                {
                    table.Columns.Add("AccountId", typeof(int));
                    table.Columns.Add("MeterReadingDateTime", typeof(DateTime));
                    table.Columns.Add("MeterReadValue", typeof(string));

                    foreach (var meterReading in readings)
                    {
                        table.Rows.Add(meterReading.AccountId, meterReading.MeterReadingDateTime, meterReading.MeterReadValue);
                    }

                    command.CommandText = "[MeterReadings].[AddReadings]";
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.Add("@newReadings", SqlDbType.Structured).Value = table;
                    var added = new List<MeterReading>();
                    await connection.OpenAsync();

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            added.Add(BuildMeterReading(reader));
                        }
                    }
                    return added;
                }
            }
        }

        public async Task<IEnumerable<MeterReading>> GetReadings()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            using (var command = connection.CreateCommand())
            {
                command.CommandText = "[MeterReadings].[GetAllReadings]";
                command.CommandType = CommandType.StoredProcedure;
                var results = new List<MeterReading>();
                await connection.OpenAsync();

                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        results.Add(BuildMeterReading(reader));
                    }
                }
                return results;
            }
        }

        public async Task<MeterReading> GetReading(Guid id)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            using (var command = connection.CreateCommand())
            {
                command.CommandText = "[MeterReadings].[GetReading]";
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.Add("@Id", SqlDbType.UniqueIdentifier).Value = id;

                await connection.OpenAsync();

                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        return BuildMeterReading(reader);
                    }
                }
            }

            return null;
        }

        public async Task DeleteReading(Guid id)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            using (var command = connection.CreateCommand())
            {
                command.CommandText = "[MeterReadings].[DeleteReading]";
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.Add("@Id", SqlDbType.UniqueIdentifier).Value = id;

                await connection.OpenAsync();
                await command.ExecuteReaderAsync();
            }
        }

        private static MeterReading BuildMeterReading(SqlDataReader reader)
        {
            return new MeterReading()
            {
                AccountId = reader.GetInt32(reader.GetOrdinal("AccountId")),
                MeterReadingDateTime = reader.GetDateTime(reader.GetOrdinal("MeterReadingDateTime")),
                MeterReadValue = reader.GetString(reader.GetOrdinal("MeterReadValue")),
                Id = reader.GetGuid(reader.GetOrdinal("Id")),
            };
        }
    }
}
