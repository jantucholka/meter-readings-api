using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using MeterReadings.Schema;

namespace MeterReadings.Repository
{
    public class AccountRepository : IAccountRepository
    {
        private readonly string connectionString = ConfigurationManager.ConnectionStrings["meterReadings"]
            .ConnectionString;

        public async Task<IEnumerable<Account>> AddAccounts(IEnumerable<Account> accounts)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            using (var command = connection.CreateCommand())
            {
                using (var table = new DataTable())
                {
                    table.Columns.Add("AccountId", typeof(int));
                    table.Columns.Add("FirstName", typeof(string));
                    table.Columns.Add("LastName", typeof(string));

                    foreach (var account in accounts)
                    {
                        table.Rows.Add(account.AccountId, account.FirstName, account.LastName);
                    }

                    command.CommandText = "[MeterReadings].[AddAccounts]";
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.Add("@newAccounts", SqlDbType.Structured).Value = table;
                    var added = new List<Account>();
                    await connection.OpenAsync();

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            added.Add(BuildAccount(reader));
                        }
                    }
                    return added;
                }
            }
        }

        public async Task<IEnumerable<Account>> GetAccounts()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            using (var command = connection.CreateCommand())
            {
                command.CommandText = "[MeterReadings].[GetAllAccounts]";
                command.CommandType = CommandType.StoredProcedure;
                var results = new List<Account>();
                await connection.OpenAsync();

                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        results.Add(BuildAccount(reader));
                    }
                }
                return results;
            }
        }

        public async Task<Account> GetAccount(int id)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            using (var command = connection.CreateCommand())
            {
                command.CommandText = "[MeterReadings].[GetAccount]";
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.Add("@AccountId", SqlDbType.Int).Value = id;

                await connection.OpenAsync();

                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        return BuildAccount(reader);
                    }
                }
            }

            return null;
        }

        public async Task DeleteAccount(int id)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            using (var command = connection.CreateCommand())
            {
                command.CommandText = "[MeterReadings].[DeleteAccount]";
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.Add("@AccountId", SqlDbType.Int).Value = id;

                await connection.OpenAsync();
                await command.ExecuteReaderAsync();
            }
        }

        private static Account BuildAccount(SqlDataReader reader)
        {
            return new Account()
            {
                AccountId = reader.GetInt32(reader.GetOrdinal("AccountId")),
                FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                LastName = reader.GetString(reader.GetOrdinal("LastName"))
            };
        }
    }
}
