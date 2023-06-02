using System.Linq;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Net.Mime;
using System.Threading.Tasks;
using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Twitter.Analytics.Domain.Accounts.Entities;
using Twitter.Analytics.Domain.Accounts.Models;
using Twitter.Analytics.Domain.Tweets;

namespace Twitter.Analytics.Api.Controllers
{
    [Route("api/v1/tweets")]
    public class TweetsController : Controller
    {
        private readonly ITweetService _tweetService;

        public TweetsController(ITweetService tweetService)
        {
            _tweetService = tweetService;
        }


        [HttpGet]
        [Produces(MediaTypeNames.Application.Json)]
        public async Task<IActionResult> Find([FromQuery] List<long> ids)
        {
            var response = await _tweetService.GetTweetsFromIds(ids);

            return Ok(response);
        }
    }
}
