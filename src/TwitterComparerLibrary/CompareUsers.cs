using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace TwitterComparerLibrary
{
    public class CompareUsers
    {
        private readonly string _token;

        public CompareUsers(string token)
        {
            _token = token;
        }

        public async Task<int> CommonFriendsNumber(string firstUserName, string secondUserName)
        {
            var commonFriendsList = await CommonFriendsList(firstUserName, secondUserName);
            return commonFriendsList.Count();
        }

        public async Task<int> CommonFollowersNumber(string firstUserName, string secondUserName)
        {
            var commonFollowersList = await CommonFollowersList(firstUserName, secondUserName);
            return commonFollowersList.Count();
        }

        public async Task<List<User>> CommonFollowersList(string firstUserName, string secondUserName)
        {
            const string url = "https://api.twitter.com/1.1/followers/list.json?screen_name=";
            return await GetIntersectList(firstUserName, secondUserName, url);
        }

        public async Task<List<User>> CommonFriendsList(string firstUserName, string secondUserName)
        {
            const string url = "https://api.twitter.com/1.1/friends/list.json?screen_name=";

            return await GetIntersectList(firstUserName, secondUserName, url);
        }

        private async Task<List<User>> GetIntersectList(string firstUserName, string secondUserName, string url)
        {
            var firstFollowers = await GetResult(url + firstUserName);
            var secondFollowers = await GetResult(url + secondUserName);

            var firstDeserialize = JsonConvert.DeserializeObject<UsersList>(firstFollowers);
            var secondDeserialize = JsonConvert.DeserializeObject<UsersList>(secondFollowers);

            var firstFollowersList = firstDeserialize.Users;
            var secondFollowersList = secondDeserialize.Users;

            return firstFollowersList.Intersect(secondFollowersList).ToList();
        }

        private async Task<string> GetResult(string url)
        {
            var httpClient = new HttpClient();

            httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _token);

            return await httpClient.GetAsync(url).Result.Content.ReadAsStringAsync();
        }

    }
}
