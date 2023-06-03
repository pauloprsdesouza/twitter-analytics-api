using System.Collections.Generic;
using System.Threading.Tasks;
using Twitter.Analytics.Domain.Accounts.Entities;

namespace Twitter.Analytics.Domain.Accounts
{
    public interface IAccountRepository
    {
        Task<Account> FindById(string accountId);
        Task<Account> Create(Account account);
        Task<List<Account>> FindAllUnprocessed(int total);
        Task<List<Account>> CreateFromList(List<Account> accounts);
    }
}
