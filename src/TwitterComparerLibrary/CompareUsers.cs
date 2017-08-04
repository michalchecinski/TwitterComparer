using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace TwitterComparerLibrary
{
    public class CompareUsers
    {
        private readonly string _token;

        public CompareUsers(string token)
        {
            _token = token;
        }

        private async Task<string> GetResult(string url)
        {
            var httpClient = new HttpClient();

            httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _token);

            return await httpClient.GetAsync(url).Result.Content.ReadAsStringAsync();
        }

        public async Task<int> CommonFriendsNumber(string firstUserName, string secondUserName)
        {
            var firstFriends = await GetResult($"https://api.twitter.com/1.1/friends/list.json?screen_name={firstUserName}");
            var secondFriends = await GetResult($"https://api.twitter.com/1.1/friends/list.json?screen_name={secondUserName}");

            return firstFriends.Intersect(secondFriends).Count();
        }

        public async Task<int> CommonFollowersNumber(string firstUserName, string secondUserName)
        {
            string url = "https://api.twitter.com/1.1/followers/list.json?screen_name=";
            var firstFriends = await GetResult(url+firstUserName);
            var secondFriends = await GetResult(url+secondUserName);

            return firstFriends.Intersect(secondFriends).Count();
        }

        

    }
}
