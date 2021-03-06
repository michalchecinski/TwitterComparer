﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
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
            ITokenProvider tokenProvider = new OAuthTwitterToken();
            string token = await tokenProvider.GetAsync(customerKey, customerSecret);
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
        public void get_authorised_response()
        {
            var results = Execute("mi_checinski").Result;

            Assert.NotEqual("{\"errors\":[{\"code\":215,\"message\":\"Bad Authentication data.\"}]}", results);
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

            JsonConvert.DeserializeObject<dynamic>(result);
        }

        [Fact]
        public void deserialize_json_object_to_User_list()
        {
            var result = Execute("spetzu").Result;

            var friendsList = JsonConvert.DeserializeObject<JsonUsersRoot>(result).Users;

            Assert.NotNull(friendsList);

            Assert.NotEmpty(friendsList);
        }

    }
}
