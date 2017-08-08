using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace TwitterComparerLibrary
{
    public class CompareUsersFriends
    {
        private readonly string _token;

        private static List<User> _lastFriendsList;

        private static DateTime _lastUpdate;

        private static string _lastFirstUser;
        private static string _lastSecondUser;

        public CompareUsersFriends(string token)
        {
            _token = token;
        }

        public async Task<int> GetCommonFriendsNumberAsync(string firstUserName, string secondUserName)
        {
            var commonFriendsList = await GetCommonFriendsListAsync(firstUserName, secondUserName);
            return commonFriendsList.Count();
        }

        public async Task<List<User>> GetCommonFriendsListAsync(string firstUserName, string secondUserName)
        {

            if (SameUserNames(firstUserName, secondUserName)  && 
                _lastUpdate >= DateTime.Now.AddMinutes(-16))
            {
                return _lastFriendsList;
            }
            const string url = "https://api.twitter.com/1.1/friends/list.json?screen_name=";
            _lastFriendsList = await new TwitterApiRequestHandler(_token).GetCommonUsersListAsync(firstUserName, secondUserName, url);
            _lastUpdate = DateTime.Now;
            _lastFirstUser = firstUserName;
            _lastSecondUser = secondUserName;

            return _lastFriendsList;
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
