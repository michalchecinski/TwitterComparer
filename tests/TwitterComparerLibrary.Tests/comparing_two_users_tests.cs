using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JsonConfig;
using Xunit;

namespace TwitterComparerLibrary.Tests
{
    public class comparing_two_users_tests
    {
        private readonly string firstUser = "maniserowicz";
        private readonly string secondUser = "spetzu";

        private readonly string _customerKey;
        private readonly string _customerSecret;
        private readonly CompareUsers _compareUsers;

        public comparing_two_users_tests()
        {
            _customerKey =  ConfigurationManager.AppSettings["CustomerKey"];
            _customerSecret = ConfigurationManager.AppSettings["CustomerSecret"];
            _compareUsers = new CompareUsers(TwitterOAuth.GetToken(_customerKey, _customerSecret).Result);
        }

        [Fact]
        public async Task common_friends_list_returns_number_greater_than_zero()
        {
            int commonFriendsNumber = await _compareUsers.CommonFriendsNumber(firstUser, secondUser);

            Assert.True(commonFriendsNumber>0);
        }

        //Do not run this test on users that have many followers because it will hit API too many times and cut your payload
        [Fact]
        public async Task common_followers_list_returns_not_empty_list()
        {
            var commonFollowers = await _compareUsers.CommonFollowersList(firstUser, secondUser);

            Assert.NotEmpty(commonFollowers);
        }

        //Do not run this test on users that have many followers because it will hit API too many times and cut your payload
        [Fact]
        public async Task common_followers_number_returns_number_greater_than_zero()
        {
            int commonFollowersNumber = await _compareUsers.CommonFollowersNumber(firstUser, secondUser);

            Assert.True(commonFollowersNumber > 0);
        }
    }
}
