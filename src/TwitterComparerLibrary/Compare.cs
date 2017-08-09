using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TwitterComparerLibrary.Model;

namespace TwitterComparerLibrary
{
    public class Compare : ICompare
    {

        private readonly string _token;

        public Compare(string token)
        {
            _token = token;
        }
        public async Task<CompareUsersResult> CompareUsers(string firstUserName, string secondUserName)
        {
            var userInformation = new UserInformation(_token);

            User firstUser = await userInformation.Get(firstUserName);
            User secondUser = await userInformation.Get(secondUserName);

            var compareUserFollowers = new CompareUsersFollowers(_token);
            var compareUserFriends = new CompareUsersFriends(_token);

            var commonFollowers = await compareUserFollowers.GetCommonFollowersListAsync(firstUserName, secondUserName);
            var commonFriends = await compareUserFriends.GetCommonFriendsListAsync(firstUserName, secondUserName);

            return new CompareUsersResult(firstUser, secondUser, commonFollowers, commonFriends);
        }
    }
}
