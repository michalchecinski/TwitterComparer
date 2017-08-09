using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TwitterComparerLibrary
{
    public class CompareUsersFollowers : ICompareUsersFollowers
    {
        private readonly string _token;

        private static readonly Cache Cache = new Cache();

        public CompareUsersFollowers(string token)
        {
            _token = token;
        }

        public async Task<int> CommonFollowersNumberAsync(string firstUserName, string secondUserName)
        {
            var commonFollowersList = await GetCommonFollowersListAsync(firstUserName, secondUserName);
            return commonFollowersList.Count();
        }

        public async Task<List<User>> GetCommonFollowersListAsync(string firstUserName, string secondUserName)
        {
            const string url = "https://api.twitter.com/1.1/followers/list.json?screen_name=";
            return await new TwitterApiRequestHandler(_token).GetCommonUsersListAsync(firstUserName, secondUserName, url, Cache);
        }
    }
}