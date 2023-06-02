using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.DynamoDBv2.Model;
using Twitter.Analytics.Infrastructure.Database.DataModel.BaseModels;

namespace Twitter.Analytics.Infrastructure.Database
{
    public class DynamoDbQueryBuilder<T> where T : BaseModel
    {
        private readonly QueryFilter filter;
        private readonly BaseKey<T> _primaryKey;
        private readonly IDynamoDBContext _dbContext;

        public DynamoDbQueryBuilder(BaseKey<T> primaryKey, IDynamoDBContext dbContext)
        {
            _primaryKey = primaryKey;
            _dbContext = dbContext;
            filter = new QueryFilter();
            filter.AddCondition(DynamoDbTable.PK, QueryOperator.Equal, primaryKey.PK);
        }

        public DynamoDbQueryBuilder<T> AddCondition(string attributeName, QueryOperator queryOperator, object value)
        {
            if (value is not null)
            {
                var attributeValue = new AttributeValue(value.ToString());

                filter.AddCondition(attributeName, queryOperator, new List<AttributeValue>() { attributeValue });
            }

            return this;
        }

        public DynamoDbQueryBuilder<T> AddCondition(string attributeName, ScanOperator scanOperator, object value)
        {
            if (value is not null)
            {
                var attributes = new List<AttributeValue>();
                if (value is ICollection)
                {
                    var items = value as ICollection;
                    foreach (var item in items)
                        attributes.Add(new AttributeValue(item.ToString()));
                }
                else
                    attributes.Add(new AttributeValue(value.ToString()));

                filter.AddCondition(attributeName, scanOperator, attributes);
            }

            return this;
        }

        public async Task<List<T>> Build(bool backwardSearch = true)
        {
            var queryConfig = new QueryOperationConfig
            {
                Filter = filter,
                BackwardSearch = backwardSearch
            };

            return await _dbContext.FromQueryAsync<T>(queryConfig).GetRemainingAsync();
        }
    }
}
