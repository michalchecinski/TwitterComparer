using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TwitterComparerLibrary
{
    public class CompareUsersFollowers
    {
        private readonly string _token;

        private static string lastFirstUser;
        private static string lastSecondUser;

        private static DateTime LastUpdate;

        private static List<User> LastFollowersList;

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
            if (lastFirstUser != firstUserName || 
                lastSecondUser != secondUserName ||  
                LastUpdate > DateTime.Now.AddMinutes(-30))
            {
                const string url = "https://api.twitter.com/1.1/followers/list.json?screen_name=";
                LastFollowersList = await TwitterApiRequestHandler.GetIntersectListAsync(firstUserName, secondUserName, url, _token);
                LastUpdate = DateTime.Now;
                lastFirstUser = firstUserName;
                lastSecondUser = secondUserName;
            }
            return LastFollowersList;
        }
    }
}