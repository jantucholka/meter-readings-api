using System.Collections.Generic;
using System.Threading.Tasks;
using MeterReadings.Schema;

namespace MeterReading.Logic
{
    public interface IAccountFacade
    {
        Task<int?> AddAccount(Account account);
        Task<IEnumerable<Account>> GetAccounts();
        Task<Account> GetAccount(int id);
        Task DeleteAccount(int id);
    }
}
