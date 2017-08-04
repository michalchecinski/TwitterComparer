using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace TwitterComparerLibrary
{
    public class UserInformation
    {
        private readonly string _token;

        public UserInformation(string token)
        {
            _token = token;
        }

        private async Task<string> GetResultAsync(string url)
        {
            var httpClient = new HttpClient();

            httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _token);

            return await httpClient.GetAsync(url).Result.Content.ReadAsStringAsync();
        }

        public async Task<User> Get(string userName)
        {
            const string url = "https://api.twitter.com/1.1/users/show.json?screen_name=";

            var json = await GetResultAsync(url + userName);

            return JsonConvert.DeserializeObject<User>(json);
        }
    }
}
