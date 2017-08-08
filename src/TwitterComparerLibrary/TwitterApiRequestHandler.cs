using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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

        public async Task<List<User>> GetCommonUsersListAsync(string firstUserName, string secondUserName, string url, Cache cache)
        {
            _i = 0;

            if (SameUserNames(firstUserName, secondUserName, cache) &&
                cache.UpdateDateTime >= DateTime.Now.AddMinutes(-16))
            {
                return cache.UsersList;
            }

            var firstList = await GetAllUsersListAsync(firstUserName, url);
            var secondList = await GetAllUsersListAsync(secondUserName, url);

            var list = GetIntersectUserList(firstList, secondList);

            cache.Update(firstUserName, secondUserName, list);

            return cache.UsersList;
        }

        private bool SameUserNames(string firstUserName, string secondUserName, Cache cache)
        {
            if (cache.FirstUser == firstUserName && cache.SecondUser == secondUserName)
            {
                return true;
            }
            if (cache.FirstUser == secondUserName && cache.SecondUser == firstUserName)
            {
                return true;
            }

            return false;

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
            {
                throw new WebException($"Twitter API rate limit exceeded after {_i} requests");
            }

            if (result.StatusCode == HttpStatusCode.NotFound)
            {
                throw new WebException("Content not found");
            }

            return await result.Content.ReadAsStringAsync();
        }
    }
}