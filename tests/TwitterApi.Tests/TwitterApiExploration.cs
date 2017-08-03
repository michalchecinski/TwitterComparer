using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Newtonsoft.Json;
using TwitterComparerLibrary;
using Xunit;

namespace TwitterApi.Tests
{
    public class TwitterApiExploration
    {
        private async Task<string> Execute(string name)
        {
            string url = $"https://api.twitter.com/1.1/friends/list.json?screen_name={name}";

            var httpClient = new HttpClient();

            string customerKey = ConfigurationManager.AppSettings["CustomerKey"];
            string customerSecret = ConfigurationManager.AppSettings["CustomerSecret"];
            string token = await TwitterOAuth.GetToken(customerKey, customerSecret);
            httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            return httpClient.GetAsync(url).Result.Content.ReadAsStringAsync().Result;
        }

        [Fact]
        public void get_not_empty_response()
        {
            var results = Execute("mi_checinski").Result;

            Console.WriteLine(results);

            Assert.NotEmpty(results);
        }

        [Fact]
        public void compare_two_list_of_followers_should_not_be_empty()
        {
            var firstResult = Execute("mi_checinski").Result;
            var secondResult = Execute("maniserowicz").Result;

            var intersect = firstResult.Intersect(secondResult);

            var enumerable = intersect as IList<char> ?? intersect.ToList();
            Console.WriteLine(enumerable.Count());

            Assert.NotEmpty(enumerable);
        }

        [Fact]
        public void returns_json_response()
        {
            var result = Execute("mi_checinski").Result;

            DeserializeJson(result);
        }

        [Fact]
        public void deserialize_json_object_to_User_list()
        {
            var result = Execute("mi_checinski").Result;

            var friendsList = JsonConvert.DeserializeObject<RootObject>(result).Users;

            Assert.NotNull(friendsList);

            Assert.NotEmpty(friendsList);
        }

        private static dynamic DeserializeJson(string result)
        {
            return JsonConvert.DeserializeObject<dynamic>(result);
        }
    }
}
