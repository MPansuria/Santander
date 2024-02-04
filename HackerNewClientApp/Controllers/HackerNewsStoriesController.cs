using AutoMapper;
using HackerNewsClientAPI.Config;
using HackerNewsClientAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Collections.Concurrent;

namespace HackerNewsClientAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class HackerNewsStoriesController : ControllerBase
    {
        private readonly ILogger<HackerNewsStoriesController> _logger;
        private readonly IMapper _mapper;
        private readonly Settings _settings;
        public HackerNewsStoriesController(ILogger<HackerNewsStoriesController> logger, IMapper mapper, IOptions<Settings> options)
        {
            _logger = logger;
            _mapper = mapper;
            _settings = options.Value;
        }

        /// <summary>
        /// Retrieves best n (numberOfStories) number of stories from the Hacker News API
        /// </summary>
        /// <param name="numberOfStories">Number of best stories to retrieve</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<StoryDto>>> GetBestStories(int numberOfStories)
        {
            try
            {
                if(numberOfStories <= 0) 
                { 
                    return BadRequest("Invalid parameter supplied."); 
                }
                ConcurrentBag<StoryDto> cb = new ConcurrentBag<StoryDto>();
                using (HttpClient client = new HttpClient())
                {
                    //Get Best Stories
                    var bestStories = await client.GetFromJsonAsync<List<int>>(_settings.EndPointBestStories);

                    //Only take IDs of n number of stories
                    var bestStoriesToProcess = bestStories?.Take(numberOfStories);

                    ParallelOptions parallelOptions = new()
                    {
                        MaxDegreeOfParallelism = _settings.MaxParallelRequests
                    };
                    await Parallel.ForEachAsync(bestStoriesToProcess
                        , parallelOptions, async (bestStory, token) =>
                        {
                            string url = _settings.EndPointStoryDetail + $"{bestStory}.json";
                            await Task.Run(async () =>
                            {
                                var story = await client.GetFromJsonAsync<Story>(url);
                                cb.Add(_mapper.Map<StoryDto>(story));
                            });
                        }
                      );

                    return Ok(cb.OrderByDescending(story => story.score));
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("An error occured.", ex);
                return StatusCode(500, ex.Message);
            }
        }
    }
}
