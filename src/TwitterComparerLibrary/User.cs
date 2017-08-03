using Newtonsoft.Json;

namespace TwitterComparerLibrary
{
    public class User
    {
        [JsonProperty("id")]
        public long Id { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("screen_name")]
        public string ScreenName { get; set; }
        [JsonProperty("url")]
        public string Url { get; set; }
        [JsonProperty("statuses_count")]
        public int StatusesCount { get; set; }
    }
}