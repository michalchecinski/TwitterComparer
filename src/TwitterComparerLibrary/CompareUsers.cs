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

        private string GetResult(string url)
        {
            var httpClient = new HttpClient();

            httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _token);

            return httpClient.GetAsync(url).Result.Content.ReadAsStringAsync().Result;
        }

        public int CommonFriendsNumber(string firstUserName, string secondUserName)
        {
            var firstFriends = GetResult($"https://api.twitter.com/1.1/friends/list.json?screen_name={firstUserName}");
            var secondFriends = GetResult($"https://api.twitter.com/1.1/friends/list.json?screen_name={secondUserName}");

            return firstFriends.Intersect(secondFriends).Count();
        }

    }
}
