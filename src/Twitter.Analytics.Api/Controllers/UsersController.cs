using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Twitter.Analytics.Domain.Accounts;
using Twitter.Analytics.Domain.Accounts.Entities;
using Twitter.Analytics.Domain.Accounts.Models;
using Twitter.Analytics.Domain.Tweets.Entities;
using Twitter.Analytics.Domain.Tweets.Models;

namespace Twitter.Analytics.Api.Controllers
{
    [Route("api/v1/accounts")]
    public class AccountsController : Controller
    {
        private readonly IAccountService _accountService;
        public AccountsController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpPost]
        public async Task<IActionResult> SaveAccountsFromCsv(IFormFile csvFile)
        {
            try
            {
                if (csvFile == null || csvFile.Length == 0)
                {
                    return BadRequest("No file uploaded.");
                }

                var config = new CsvConfiguration(CultureInfo.InvariantCulture)
                {
                };

                using (var reader = new StreamReader(csvFile.OpenReadStream()))
                using (var csv = new CsvReader(reader, config))
                {
                    csv.Context.RegisterClassMap<AccountMap>();
                    var tweets = csv.GetRecords<Account>().ToList();

                    await _accountService.CreateFromList(tweets);

                    return Ok(tweets);
                }
            }
            catch (System.Exception ex)
            {
                return NoContent();
            }
        }
    }
}
