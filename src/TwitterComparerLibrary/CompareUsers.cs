using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace TwitterComparerLibrary
{
    public class CompareUsers
    {
        private readonly string _token;

        private int _i;

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
            List<User> firstFollowersList = new List<User>();
            JsonUsersRoot firstDeserialize = new JsonUsersRoot();

            _i = 0;

            while (firstDeserialize.NextCursor != 0)
            {
                _i++;
                var firstFollowers = await GetResult(url + firstUserName + "&cursor="+ firstDeserialize.NextCursor);
                firstDeserialize = JsonConvert.DeserializeObject<JsonUsersRoot>(firstFollowers);
                var temp = firstFollowersList.Concat(firstDeserialize.Users);
                firstFollowersList = temp.ToList();
            }

            List<User> secondFollowersList = new List<User>();
            JsonUsersRoot secondDeserialize = new JsonUsersRoot();

            while (secondDeserialize.NextCursor != 0)
            {
                _i++;
                var secondFollowers = await GetResult(url + secondUserName + "&cursor=" + secondDeserialize.NextCursor);
                secondDeserialize = JsonConvert.DeserializeObject<JsonUsersRoot>(secondFollowers);
                var temp = secondFollowersList.Concat(secondDeserialize.Users);
                secondFollowersList = temp.ToList();
            }

            return firstFollowersList.Intersect(secondFollowersList).ToList();
        }

        private async Task<string> GetResult(string url)
        {
            var httpClient = new HttpClient();

            httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _token);

            var result = await httpClient.GetAsync(url);

            if(result.StatusCode == ((System.Net.HttpStatusCode)429))
                throw new TimeoutException($"Twitter API rate limit exceeded after {_i} requests");

            return await result.Content.ReadAsStringAsync();
        }

    }
}
