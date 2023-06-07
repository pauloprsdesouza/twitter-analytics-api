using System.Collections.Generic;
using System.Threading.Tasks;
using Twitter.Analytics.Domain.Accounts.Entities;

namespace Twitter.Analytics.Domain.Accounts
{
    public interface IAccountService
    {
        Task<List<Account>> FindAllUnprocessed();
        Task<Account> Create(Account account);
        Task<List<Account>> CreateFromList(List<Account> accounts);
    }
}
