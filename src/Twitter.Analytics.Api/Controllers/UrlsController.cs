using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Mime;
using System.Threading.Tasks;
using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Twitter.Analytics.Domain.Tweets;
using Twitter.Analytics.Domain.Urls;
using Twitter.Analytics.Domain.Urls.Models;

namespace Twitter.Analytics.Api.Controllers
{
    [Route("api/v1/urls")]
    public class UrlsController : Controller
    {
        private readonly ITweetService _tweetService;

        public UrlsController(ITweetService tweetService)
        {
            _tweetService = tweetService;
        }


        [HttpPost, Route("csv")]
        [Produces(MediaTypeNames.Application.Json)]
        public async Task<IActionResult> SaveTweetsFromCsv(IFormFile csvFile)
        {
            try
            {
                if (csvFile == null || csvFile.Length == 0)
                {
                    return BadRequest("No file uploaded.");
                }

                var config = new CsvConfiguration(CultureInfo.InvariantCulture)
                {
                    BadDataFound = null,
                };


                using (var reader = new StreamReader(csvFile.OpenReadStream()))
                using (var csv = new CsvReader(reader, config))
                {
                    csv.Context.RegisterClassMap<UrlMap>();
                    var urls = csv.GetRecords<UrlModel>().ToList();

                    await _tweetService.UpdateUrls(urls);

                    return Ok();
                }
            }
            catch (System.Exception ex)
            {
                return NoContent();

            }
        }
    }
}
