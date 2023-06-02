using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Microsoft.Extensions.DependencyInjection;

namespace Twitter.Analytics.Api.DependencyInjection
{
    public static class DynamoDBDependency
    {
        public static void AddDynamoDBDependency(this IServiceCollection services)
        {
            services.AddAWSService<IAmazonDynamoDB>();
            services.AddScoped<IDynamoDBContext, DynamoDBContext>();
        }
    }
}
