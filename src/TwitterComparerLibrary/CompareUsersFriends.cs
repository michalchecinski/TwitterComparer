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
        private static string _token;

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
            if (_lastFirstUser != firstUserName ||
                _lastSecondUser != secondUserName ||
                _lastUpdate > DateTime.Now.AddMinutes(-30))
            {
                const string url = "https://api.twitter.com/1.1/friends/list.json?screen_name=";
                _lastFriendsList = await new TwitterApiRequestHandler(_token).GetCommonUserstListAsync(firstUserName, secondUserName, url);
                _lastUpdate = DateTime.Now;
                _lastFirstUser = firstUserName;
                _lastSecondUser = secondUserName;
            }

            return _lastFriendsList;
        }

    }
}
