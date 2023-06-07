using Twitter.Analytics.Infrastructure.Database.DataModel.BaseModels;

namespace Twitter.Analytics.Infrastructure.Database.DataModel.Users
{
    public class AccountKey : BaseKey<AccountModel>
    {
        public AccountKey(AccountModel accountModel)
        {
            PK = $"Account";
            SK = $"{accountModel.Id}";
        }

        public AccountKey()
        {
            PK = $"Account";
        }
    }
}
