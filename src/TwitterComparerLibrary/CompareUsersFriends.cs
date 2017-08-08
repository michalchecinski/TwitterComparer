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

        private static readonly Cache _cache = new Cache();

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
                _cache.UpdateDateTime >= DateTime.Now.AddMinutes(-16))
            {
                return _cache.UsersList;
            }
            const string url = "https://api.twitter.com/1.1/friends/list.json?screen_name=";
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
