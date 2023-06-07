using System.Collections.Generic;
using System.Threading.Tasks;
using Twitter.Analytics.Domain.Accounts.Entities;

namespace Twitter.Analytics.Domain.Accounts
{
    public interface IAccountRepository
    {
        Task<Account> Create(Account account);
        Task<Account> Update(Account account);
        Task<List<Account>> FindAllUnprocessed();
        Task<List<Account>> CreateFromList(List<Account> accounts);
        Task<List<Account>> UpdateFromList(List<Account> accounts);
    }
}
