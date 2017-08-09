using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using JsonConfig;
using Xunit;
using Xunit.Sdk;

namespace TwitterComparerLibrary.Tests
{
    public class comparing_two_users_followers_tests
    {

        //Do not run those tests on users that have many followers(more than 300 followers in practice) 
        //because it will hit API too many times and will exceed API rate limit
        private const string _firstUser = "MDziubiak";
        private const string _secondUser = "mi_checinski";

        private readonly CompareUsersFollowers _compareUsersFollowers;

        public comparing_two_users_followers_tests()
        {
            var customerKey =  ConfigurationManager.AppSettings["CustomerKey"];
            var customerSecret = ConfigurationManager.AppSettings["CustomerSecret"];
            _compareUsersFollowers = new CompareUsersFollowers(OAuthTwitterToken.GetAsync(customerKey, customerSecret).Result);
        }

        [Fact]
        public async Task common_followers_list_returns_not_empty_list()
        {
            var commonFollowers = await _compareUsersFollowers.CommonFollowersListAsync(_firstUser, _secondUser);

            Assert.NotEmpty(commonFollowers);
        }

        [Fact]
        public async Task common_followers_number_returns_number_greater_than_zero()
        {
            int commonFollowersNumber = await _compareUsersFollowers.CommonFollowersNumberAsync(_firstUser, _secondUser);

            Assert.True(commonFollowersNumber > 0);
        }

        [Fact]
        public async Task web_exception_thrown_when_one_not_user()
        {
           await Assert.ThrowsAsync<WebException>(async () => 
                await _compareUsersFollowers.CommonFollowersListAsync(_firstUser, "ofksofs"));

        }
    }
}
