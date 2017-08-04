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

        public comparing_two_users_tests()
        {
            _customerKey =  ConfigurationManager.AppSettings["CustomerKey"];
            _customerSecret = ConfigurationManager.AppSettings["CustomerSecret"];
        }

        [Fact]
        public void common_friends_list_returns_number()
        {
            CompareUsers compareUsers = new CompareUsers(TwitterOAuth.GetToken(_customerKey, _customerSecret).Result);

            int commonFriendsNumber = compareUsers.CommonFriendsNumber("mi_checinski", "maniserowicz");

            Assert.True(commonFriendsNumber>0);
        }
    }
}
