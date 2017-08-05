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

        public async Task<User> Get(string userName)
        {
            const string url = "https://api.twitter.com/1.1/users/show.json?screen_name=";

            var json = await TwitterApiRequestHandler.GetResultAsync(url + userName, _token);

            return JsonConvert.DeserializeObject<User>(json);
        }
    }
}
