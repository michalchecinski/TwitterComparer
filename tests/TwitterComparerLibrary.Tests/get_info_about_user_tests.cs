using System.Configuration;
using System.Net;
using System.Threading.Tasks;
using Shouldly;
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
            ITokenProvider tokenProvider = new OAuthTwitterToken();
            _userInformation = new UserInformation(tokenProvider.GetAsync(customerKey, customerSecret).Result);
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

            user.ShouldBe(expectedUser);
        }

        [Fact]
        public async Task web_exception_thrown_when_user_not_found()
        {
            await Should.ThrowAsync<WebException>(async () =>
                await _userInformation.Get("ofksofs"));
        }

        [Fact]
        public async Task not_existing_user_should_be_false()
        {
            var result = await _userInformation.UserExistsAsync("ofksofs");

            result.ShouldBeFalse();
        }

        [Fact]
        public async Task existing_user_should_be_true()
        {
            var result = await _userInformation.UserExistsAsync("mi_checinski");

            result.ShouldBeTrue();
        }

    }
}
