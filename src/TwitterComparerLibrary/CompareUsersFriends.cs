﻿using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TwitterComparerLibrary
{
    public class CompareUsersFriends
    {
        private readonly string _token;

        private static readonly Cache Cache = new Cache();

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
            const string url = "https://api.twitter.com/1.1/friends/list.json?screen_name=";
            return await new TwitterApiRequestHandler(_token).GetCommonUsersListAsync(firstUserName, secondUserName, url, Cache);
        }

    }
}
