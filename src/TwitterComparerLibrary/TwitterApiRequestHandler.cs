using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace TwitterComparerLibrary
{
    public class TwitterApiRequestHandler
    {
        private static int _i;

        public static async Task<List<User>> GetIntersectListAsync(string firstUserName, string secondUserName, string url, string token)
        {

            List<User> firstFollowersList = new List<User>();
            JsonUsersRoot firstDeserialize = new JsonUsersRoot();

            _i = 0;

            while (firstDeserialize.NextCursor != 0)
            {
                _i++;
                var firstFollowers = await GetResultAsync(url + firstUserName + "&cursor="+ firstDeserialize.NextCursor, token);
                firstDeserialize = JsonConvert.DeserializeObject<JsonUsersRoot>(firstFollowers);
                var temp = firstFollowersList.Concat(firstDeserialize.Users);
                firstFollowersList = temp.ToList();
            }

            List<User> secondFollowersList = new List<User>();
            JsonUsersRoot secondDeserialize = new JsonUsersRoot();

            while (secondDeserialize.NextCursor != 0)
            {
                _i++;
                var secondFollowers = await GetResultAsync(url + secondUserName + "&cursor=" + secondDeserialize.NextCursor, token);
                secondDeserialize = JsonConvert.DeserializeObject<JsonUsersRoot>(secondFollowers);
                var temp = secondFollowersList.Concat(secondDeserialize.Users);
                secondFollowersList = temp.ToList();
            }

            return firstFollowersList.Intersect(secondFollowersList).ToList();
        }

        public static async Task<string> GetResultAsync(string url, string token)
        {
            HttpResponseMessage result;
            using (var httpClient = new HttpClient())
            {

                httpClient.DefaultRequestHeaders.Authorization =
                    new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

                result = await httpClient.GetAsync(url);
            }

            if (result.StatusCode == ((System.Net.HttpStatusCode) 429))
                throw new Exception($"Twitter API rate limit exceeded after {_i} requests");

            return await result.Content.ReadAsStringAsync();
        }
    }
}