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
        public async Task common_friends_list_returns_number()
        {
            int commonFriendsNumber = await _compareUsers.CommonFriendsNumber("mi_checinski", "maniserowicz");

            Assert.True(commonFriendsNumber>0);
        }

        [Fact]
        public async Task common_followers_list_returns_number()
        {
            int commonFollowersNumber = await _compareUsers.CommonFollowersNumber("mi_checinski", "maniserowicz");

            Assert.True(commonFollowersNumber > 0);
        }
    }
}
