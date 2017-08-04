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
        [JsonProperty("followers_count")]
        public int FollowersCount { get; set; }
        [JsonProperty("friends_count")]
        public int FriendsCount { get; set; }

        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }

            User u = obj as User;

            if ((System.Object)u == null)
            {
                return false;
            }

            return u.Id == Id;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}