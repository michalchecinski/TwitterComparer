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

        public static async Task<List<User>> GetCommonUserstListAsync(string firstUserName, string secondUserName, string url, string token)
        {
            _i = 0;

            List<User> firstList = await GetAllUsersListAsync(firstUserName, url, token);

            List<User> secondList = await GetAllUsersListAsync(secondUserName, url, token);

            return GetIntersectUserList(firstList, secondList);
        }

        private static async Task<List<User>> GetAllUsersListAsync(string userName, string url, string token)
        {
            JsonUsersRoot firstDeserialize = new JsonUsersRoot();
            List<User> firstList = new List<User>();
            while (firstDeserialize.NextCursor != 0)
            {
                _i++;
                var firstResult = await GetResultAsync(url + userName + "&cursor=" + firstDeserialize.NextCursor, token);
                firstDeserialize = JsonConvert.DeserializeObject<JsonUsersRoot>(firstResult);
                var temp = firstList.Concat(firstDeserialize.Users);
                firstList = temp.ToList();
            }

            return firstList;
        }

        private static List<User> GetIntersectUserList(IEnumerable<User> firstList, IEnumerable<User> secondList)
        {
            return firstList.Intersect(secondList).ToList();
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