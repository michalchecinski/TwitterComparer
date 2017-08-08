using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TwitterComparerLibrary
{
    public class CompareUsersFollowers
    {
        private readonly string _token;

        private static string _lastFirstUser;
        private static string _lastSecondUser;

        private static DateTime _lastUpdate;

        private static List<User> _lastFollowersList;

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
                _lastUpdate >= DateTime.Now.AddMinutes(-16))
            {
                return _lastFollowersList;
            }
            const string url = "https://api.twitter.com/1.1/followers/list.json?screen_name=";
            _lastFollowersList = await new TwitterApiRequestHandler(_token).GetCommonUsersListAsync(firstUserName, secondUserName, url);
            _lastUpdate = DateTime.Now;
            _lastFirstUser = firstUserName;
            _lastSecondUser = secondUserName;
            return _lastFollowersList;
        }


        private bool SameUserNames(string firstUserName, string secondUserName)
        {
            if (_lastFirstUser == firstUserName && _lastSecondUser == secondUserName)
            {
                return true;
            }
            if (_lastFirstUser == secondUserName && _lastSecondUser == firstUserName)
            {
                return true;
            }

            return false;

        }
    }
}