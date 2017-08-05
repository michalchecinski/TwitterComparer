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

        private static List<User> LastFriendsList;

        private static DateTime LastUpdate;

        private static string lastFirstUser;
        private static string lastSecondUser;
        private readonly TwitterApiRequestHandler _twitterApiRequestHandler;

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
            if (lastFirstUser != firstUserName ||
                lastSecondUser != secondUserName ||
                LastUpdate > DateTime.Now.AddMinutes(-30))
            {
                const string url = "https://api.twitter.com/1.1/friends/list.json?screen_name=";
                LastFriendsList = await TwitterApiRequestHandler.GetIntersectListAsync(firstUserName, secondUserName, url, _token);
                LastUpdate = DateTime.Now;
                lastFirstUser = firstUserName;
                lastSecondUser = secondUserName;
            }

            return LastFriendsList;
        }

    }
}
