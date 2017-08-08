using System;
using System.Net;
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

            var json = await new TwitterApiRequestHandler(_token).GetResultAsync(url + userName);

            return JsonConvert.DeserializeObject<User>(json);
        }

        public async Task<bool> UserExistsAsync(string userName)
        {
            try
            {
                await Get(userName);
            }
            catch (WebException e)
            {
                if (e.Message == "Content not found")
                {
                    return false;
                }
                throw;

            }
            return true;
        }

    }
}
