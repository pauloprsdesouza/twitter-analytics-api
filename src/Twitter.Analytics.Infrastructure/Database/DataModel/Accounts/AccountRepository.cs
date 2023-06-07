using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
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

            var primaryKey = new AccountKey();
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
                var primaryKey = new AccountKey(account);
                primaryKey.AssignTo(account);
            }

            batch.AddPutItems(accountsModel);
            await batch.ExecuteAsync();

            return accounts;
        }

        public async Task<List<Account>> FindAllUnprocessed()
        {
            var primaryKey = new AccountKey();
            var accountsModel = await new DynamoDbQueryBuilder<AccountModel>(primaryKey, _dbContext)
                                        .AddCondition(nameof(AccountModel.IsProcessed), QueryOperator.Equal, new DynamoDBBool(false))
                                        .Build();

            var accounts = _mapper.Map<List<Account>>(accountsModel);

            return accounts;
        }

        public async Task<Account> Update(Account account)
        {
            var accountModel = _mapper.Map<AccountModel>(account);

            var primaryKey = new AccountKey(accountModel);
            primaryKey.AssignTo(accountModel);

            await _dbContext.SaveAsync(accountModel);

            return account;
        }

        public async Task<List<Account>> UpdateFromList(List<Account> accounts)
        {
            var batch = _dbContext.CreateBatchWrite<AccountModel>();
            var accountsModel = _mapper.Map<List<AccountModel>>(accounts.DistinctBy(x => x.Id).ToList());

            foreach (var account in accountsModel)
            {
                var primaryKey = new AccountKey(account);
                primaryKey.AssignTo(account);
            }

            batch.AddPutItems(accountsModel);
            await batch.ExecuteAsync();

            return accounts;
        }
    }
}
