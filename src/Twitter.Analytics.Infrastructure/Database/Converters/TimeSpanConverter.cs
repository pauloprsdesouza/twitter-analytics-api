using System;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;

namespace Twitter.Analytics.Infrastructure.Database.Converters
{
    public sealed class TimeSpanConverter : IPropertyConverter
    {
        const string Format = @"hh\:mm";

        public object FromEntry(DynamoDBEntry entry)
        {
            var entryAsString = entry?.AsString();

            if (entryAsString == null)
            {
                return null;
            }

            return TimeSpan.ParseExact(entryAsString, Format, null);
        }

        public DynamoDBEntry ToEntry(object value)
        {
            return ((DateTimeOffset)value).ToString(Format);
        }
    }
}
