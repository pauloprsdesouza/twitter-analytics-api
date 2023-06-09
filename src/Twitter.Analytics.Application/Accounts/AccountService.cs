using System.Collections.Generic;
using System.Threading.Tasks;
using Twitter.Analytics.Domain.Accounts;
using Twitter.Analytics.Domain.Accounts.Entities;

namespace Twitter.Analytics.Application.Accounts
{
    public class AccountService : IAccountService
    {
        private readonly IAccountRepository _accountRepository;

        public AccountService(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }

        public async Task<Account> Create(Account account)
        {
            return await _accountRepository.Create(account);
        }

        public async Task<List<Account>> CreateFromList(List<Account> accounts)
        {
            return await _accountRepository.CreateFromList(accounts);
        }

        public async Task<List<Account>> FindAllUnprocessed()
        {
            return await _accountRepository.FindAllUnprocessed();
        }
    }
}
