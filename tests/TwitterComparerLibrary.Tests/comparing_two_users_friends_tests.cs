﻿using System.Configuration;
using System.Net;
using System.Threading.Tasks;
using Shouldly;
using Xunit;

namespace TwitterComparerLibrary.Tests
{
    public class comparing_two_users_friends_tests
    {
        //Do not run those tests on users that have many friends(more than 300 friends in practice) 
        //because it will hit API too many times and will exceed API rate limit

        private readonly CompareUsersFriends _compareUsersFriends;

        private const string _firstUser = "mi_checinski";
        private const string _secondUser = "MDziubiak";

        public comparing_two_users_friends_tests()
        {
            var customerKey = ConfigurationManager.AppSettings["CustomerKey"];
            var customerSecret = ConfigurationManager.AppSettings["CustomerSecret"];
            ITokenProvider tokenProvider = new OAuthTwitterToken();
            _compareUsersFriends = new CompareUsersFriends(tokenProvider.GetAsync(customerKey, customerSecret).Result);
        }

        [Fact]
        public async Task common_friends_list_returns_number_greater_than_zero()
        {
            int commonFriendsNumber = await _compareUsersFriends.GetCommonFriendsNumberAsync(_firstUser, _secondUser);

            commonFriendsNumber.ShouldBeGreaterThan(0);
        }

        [Fact]
        public async Task common_friends_list_returns_not_empty_list()
        {
            var commonFollowers = await _compareUsersFriends.GetCommonFriendsListAsync(_firstUser, _secondUser);

            commonFollowers.ShouldNotBeEmpty();
        }

        [Fact]
        public async Task web_exception_thrown_when_one_not_user()
        {
            await Should.ThrowAsync<WebException>(async ()
                => await _compareUsersFriends.GetCommonFriendsListAsync(_firstUser, "ofksofs"));

        }

    }
}
