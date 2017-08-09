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
        public async Task<ComparedUsersDto> CompareUsers(string firstUserName, string secondUserName, string token)
        {
            var userInformation = new UserInformation(token);

            User firstUser = await userInformation.Get(firstUserName);
            User secondUser = await userInformation.Get(secondUserName);

            var compareUserFollowers = new CompareUsersFollowers(token);
            var compareUserFriends = new CompareUsersFriends(token);

            var commonFollowers = await compareUserFollowers.GetCommonFollowersListAsync(firstUserName, secondUserName);
            var commonFriends = await compareUserFriends.GetCommonFriendsListAsync(firstUserName, secondUserName);

            return new ComparedUsersDto(firstUser, secondUser, commonFollowers, commonFriends);
        }
    }
}
