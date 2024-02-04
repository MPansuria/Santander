namespace HackerNewsClientAPI.Models
{
    public class StoryDto
    {
        public string title { get; set; }
        public string uri { get; set; }
        public string postedBy { get; set; }
        public int time { get; set; }
        public int score { get; set; }
        public int commentCount { get; set; }
    }
}
