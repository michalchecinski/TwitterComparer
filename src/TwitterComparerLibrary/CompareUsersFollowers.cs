using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TwitterComparerLibrary
{
    public class CompareUsersFollowers
    {
        private readonly string _token;

       private static readonly Cache _cache = new Cache();

        public CompareUsersFollowers(string token)
        {
            _token = token;
        }

        public async Task<int> CommonFollowersNumber(string firstUserName, string secondUserName)
        {
            var commonFollowersList = await CommonFollowersList(firstUserName, secondUserName);
            return commonFollowersList.Count();
        }

        public async Task<List<User>> CommonFollowersList(string firstUserName, string secondUserName)
        {
            if (SameUserNames(firstUserName, secondUserName) &&
                _cache.UpdateDateTime >= DateTime.Now.AddMinutes(-16))
            {
                return _cache.UsersList;
            }
            const string url = "https://api.twitter.com/1.1/followers/list.json?screen_name=";
            var list = await new TwitterApiRequestHandler(_token).GetCommonUsersListAsync(firstUserName, secondUserName, url);
            _cache.Update(firstUserName, secondUserName, list);
            return _cache.UsersList;
        }


        private bool SameUserNames(string firstUserName, string secondUserName)
        {
            if (_cache.FirstUser == firstUserName && _cache.SecondUser == secondUserName)
            {
                return true;
            }
            if (_cache.FirstUser == secondUserName && _cache.SecondUser == firstUserName)
            {
                return true;
            }

            return false;

        }
    }
}