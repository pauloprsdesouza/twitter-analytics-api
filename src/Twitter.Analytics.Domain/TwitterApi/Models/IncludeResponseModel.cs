using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Twitter.Analytics.Domain.TwitterApi.Models.Users;

namespace Twitter.Analytics.Domain.TwitterApi.Models
{
    public class IncludeResponseModel
    {
        public List<UserModel> Users { get; set; }
    }
}
