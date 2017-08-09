using System.Configuration;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace TwitterComparerLibrary.Tests
{
    public class get_info_about_user_tests
    {
        private readonly UserInformation _userInformation;

        public get_info_about_user_tests()
        {
            var customerKey = ConfigurationManager.AppSettings["CustomerKey"];
            var customerSecret = ConfigurationManager.AppSettings["CustomerSecret"];
            _userInformation = new UserInformation(OAuthTwitterToken.GetAsync(customerKey, customerSecret).Result);
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

        [Fact]
        public async Task web_exception_thrown_when_user_not_found()
        {
            await Assert.ThrowsAsync<WebException>(async () => 
                    await _userInformation.Get("ofksofs"));
        }

        [Fact]
        public async Task not_existing_user_should_be_false()
        {
            var result = await _userInformation.UserExistsAsync("ofksofs");

            Assert.False(result);
        }

        [Fact]
        public async Task existing_user_should_be_true()
        {
            var result = await _userInformation.UserExistsAsync("mi_checinski");

            Assert.True(result);
        }

    }
}
