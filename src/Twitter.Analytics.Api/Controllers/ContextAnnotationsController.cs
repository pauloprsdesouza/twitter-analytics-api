using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Mime;
using System.Threading.Tasks;
using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Twitter.Analytics.Domain.Annotations.Models;
using Twitter.Analytics.Domain.Tweets;

namespace Twitter.Analytics.Api.Controllers
{
    [Route("api/v1/annotations")]
    public class ContextAnnotationsController : Controller
    {
        private readonly ITweetService _tweetService;

        public ContextAnnotationsController(ITweetService tweetService)
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
                    csv.Context.RegisterClassMap<AnnotationMap>();
                    var annotations = csv.GetRecords<AnnotationModel>().ToList();

                    await _tweetService.UpdateAnnotations(annotations);

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
