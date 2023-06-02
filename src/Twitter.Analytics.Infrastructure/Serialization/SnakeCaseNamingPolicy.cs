using System.Text;
using System.Text.Json;

namespace Twitter.Analytics.Infrastructure.Serialization
{
    public class SnakeCaseNamingPolicy : JsonNamingPolicy
    {
        public override string ConvertName(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                return name;
            }

            string convertedName = "";

            for (int i = 0; i < name.Length; i++)
            {
                if (i > 0 && char.IsUpper(name[i]))
                {
                    convertedName += "_";
                }

                convertedName += char.ToLower(name[i]);
            }

            return convertedName;
        }
    }
}
