using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace TwitterComparerLibrary
{
    internal class TwitterApiRequestHandler
    {
        private static int _requestNumber;

        private readonly string _token;

        public TwitterApiRequestHandler(string token)
        {
            _token = token;
        }

        public async Task<List<User>> GetCommonUsersListAsync(string firstUserName, string secondUserName, string url)
        {
            _requestNumber = 0;

            var firstUserExist = await new UserInformation(_token).UserExistsAsync(firstUserName);
            var secondUserExist = await new UserInformation(_token).UserExistsAsync(secondUserName);

            if (firstUserExist == false)
            {
                throw new WebException($"User {firstUserName} does not exist");
            }
            if(secondUserExist == false)
            {
                throw new WebException($"User {secondUserName} does not exist");
            }

            var firstList = await GetAllUsersListAsync(firstUserName, url);
            var secondList = await GetAllUsersListAsync(secondUserName, url);

            var list = GetIntersectUserList(firstList, secondList);

            return list;
        }

        private async Task<List<User>> GetAllUsersListAsync(string userName, string url)
        {
            var firstDeserialize = new JsonUsersRoot();
            var userList = new List<User>();
            while (firstDeserialize.NextCursor != 0)
            {
                _requestNumber++;
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
                throw new WebException($"Twitter API rate limit exceeded after {_requestNumber} requests");
            }

            if (result.StatusCode == HttpStatusCode.NotFound)
            {
                throw new WebException("Content not found");
            }

            return await result.Content.ReadAsStringAsync();
        }
    }
}