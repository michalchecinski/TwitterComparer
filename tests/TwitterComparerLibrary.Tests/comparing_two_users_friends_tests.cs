using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace TwitterComparerLibrary.Tests
{
    public class comparing_two_users_friends_tests
    {
        private readonly CompareUsersFriends _compareUsersFriends;

        private const string _firstUser = "mi_checinski";
        private const string _secondUser = "rotsap_";

        public comparing_two_users_friends_tests()
        {
            var customerKey = ConfigurationManager.AppSettings["CustomerKey"];
            var customerSecret = ConfigurationManager.AppSettings["CustomerSecret"];
            _compareUsersFriends = new CompareUsersFriends(OAuthToken.Generate(customerKey, customerSecret).Result);
        }

        [Fact]
        public async Task common_friends_list_returns_number_greater_than_zero()
        {
            int commonFriendsNumber = await _compareUsersFriends.GetCommonFriendsNumberAsync(_firstUser, _secondUser);

            Assert.True(commonFriendsNumber > 0);
        }

        [Fact]
        public async Task common_friends_list_returns_not_empty_list()
        {
            var commonFollowers = await _compareUsersFriends.GetCommonFriendsListAsync(_firstUser, _secondUser);

            Assert.NotEmpty(commonFollowers);
        }

    }
}
