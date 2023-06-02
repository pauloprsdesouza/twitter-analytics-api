using Twitter.Analytics.Infrastructure.Database.DataModel.BaseModels;

namespace Twitter.Analytics.Infrastructure.Database.DataModel.Users
{
    public class AccountKey : BaseKey<AccountModel>
    {
        public AccountKey(string accountId)
        {
            PK = $"Account";
            SK = $"Id#{accountId}";
        }

        public AccountKey()
        {
            PK = $"Account";
        }
    }
}
