using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Amazon.DynamoDBv2.DataModel;
using AutoMapper;
using Twitter.Analytics.Domain.Accounts;
using Twitter.Analytics.Domain.Accounts.Entities;

namespace Twitter.Analytics.Infrastructure.Database.DataModel.Users
{
    public class AccountRepository : IAccountRepository
    {
        private readonly IDynamoDBContext _dbContext;
        private readonly IMapper _mapper;

        public AccountRepository(IDynamoDBContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<Account> Create(Account account)
        {
            var accountModel = _mapper.Map<AccountModel>(account);

            var primaryKey = new AccountKey(account.Id);
            primaryKey.AssignTo(accountModel);

            await _dbContext.SaveAsync(accountModel);

            return account;
        }

        public async Task<List<Account>> CreateFromList(List<Account> accounts)
        {
            var batch = _dbContext.CreateBatchWrite<AccountModel>();
            var accountsModel = _mapper.Map<List<AccountModel>>(accounts.DistinctBy(x => x.Id).ToList());

            foreach (var account in accountsModel)
            {
                var primaryKey = new AccountKey(account.Id);
                primaryKey.AssignTo(account);
            }

            batch.AddPutItems(accountsModel);
            await batch.ExecuteAsync();

            return accounts;
        }

        public async Task<Account> FindById(string accountId)
        {
            var primaryKey = new AccountKey(accountId);
            var accountModel = await _dbContext.LoadAsync<AccountModel>(primaryKey.PK, primaryKey.SK);

            var account = _mapper.Map<Account>(accountModel);

            return account;
        }
    }
}
