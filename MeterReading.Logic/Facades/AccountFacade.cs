using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MeterReadings.Repository;
using MeterReadings.Schema;

namespace MeterReading.Logic.Facades
{
    public class AccountFacade : IAccountFacade
    {
        private readonly IAccountRepository _accountRepository;

        public AccountFacade(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository ?? throw new ArgumentNullException(nameof(accountRepository));
        }

        public async Task<int?> AddAccount(Account account)
        {
            var accountIds = await _accountRepository.AddAccounts(new []{account
            });
            return accountIds.FirstOrDefault()?.AccountId;
        }
            

        public Task<IEnumerable<Account>> GetAccounts() => _accountRepository.GetAccounts();

        public Task<Account> GetAccount(int id) => _accountRepository.GetAccount(id);

        public Task DeleteAccount(int id) => _accountRepository.DeleteAccount(id);
    }
}
