using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MeterReadings.Schema;

namespace MeterReadings.Repository
{
    public interface IAccountRepository
    {
        Task<IEnumerable<Account>> AddAccounts(IEnumerable<Account> accounts);
        Task<IEnumerable<Account>> GetAccounts();
        Task<Account> GetAccount(int id);
        Task DeleteAccount(int id);
    }
}