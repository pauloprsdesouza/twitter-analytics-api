using System;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;

namespace Twitter.Analytics.Infrastructure.Database.Converters
{
    public sealed class EnumConverter<TEnum> : IPropertyConverter where TEnum : Enum
    {
        public object FromEntry(DynamoDBEntry entry)
        {
            var entryAsString = entry?.AsString();

            if (entryAsString == null)
            {
                return null;
            }

            return (TEnum)Enum.Parse(typeof(TEnum), entryAsString, ignoreCase: true);
        }

        public DynamoDBEntry ToEntry(object value)
        {
            return ((TEnum)value).ToString();
        }
    }
}
