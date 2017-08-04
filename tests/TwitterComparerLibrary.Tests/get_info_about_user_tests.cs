using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace TwitterComparerLibrary.Tests
{
    public class get_info_about_user_tests
    {
        private readonly string _customerKey;
        private readonly string _customerSecret;
        private readonly UserInformation _userInformation;

        public get_info_about_user_tests()
        {
            _customerKey = ConfigurationManager.AppSettings["CustomerKey"];
            _customerSecret = ConfigurationManager.AppSettings["CustomerSecret"];
            _userInformation = new UserInformation(TwitterOAuth.GetToken(_customerKey, _customerSecret).Result);
        }

        [Fact]
        public async Task get_info_about_user()
        {
            var user = await _userInformation.Get("mi_checinski");

            User expectedUser = new User()
            {
                ScreenName = "mi_checinski",
                Id = 1380219330
            };

            Assert.Equal(expectedUser, user);
        }

    }
}
