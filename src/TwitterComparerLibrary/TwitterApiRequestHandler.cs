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

        private readonly string _token;

        public TwitterApiRequestHandler(string token)
        {
            _token = token;
        }

        public async Task<List<User>> GetCommonUsersListAsync(string firstUserName, string secondUserName, string url)
        {
            _i = 0;

            var firstList = await GetAllUsersListAsync(firstUserName, url);

            var secondList = await GetAllUsersListAsync(secondUserName, url);

            return GetIntersectUserList(firstList, secondList);
        }

        private async Task<List<User>> GetAllUsersListAsync(string userName, string url)
        {
            var firstDeserialize = new JsonUsersRoot();
            var userList = new List<User>();
            while (firstDeserialize.NextCursor != 0)
            {
                _i++;
                var firstResult = await GetResultAsync(url + userName + "&cursor=" + firstDeserialize.NextCursor);
                firstDeserialize = JsonConvert.DeserializeObject<JsonUsersRoot>(firstResult);
                var temp = userList.Concat(firstDeserialize.Users);
                userList = temp.ToList();
            }

            return userList;
        }

        private static List<User> GetIntersectUserList(IEnumerable<User> firstList, IEnumerable<User> secondList)
        {
            return firstList.Intersect(secondList).ToList();
        }

        public async Task<string> GetResultAsync(string url)
        {
            HttpResponseMessage result;
            using (var httpClient = new HttpClient())
            {

                httpClient.DefaultRequestHeaders.Authorization =
                    new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _token);

                result = await httpClient.GetAsync(url);
            }

            if (result.StatusCode == ((System.Net.HttpStatusCode) 429))
                throw new Exception($"Twitter API rate limit exceeded after {_i} requests");

            return await result.Content.ReadAsStringAsync();
        }
    }
}