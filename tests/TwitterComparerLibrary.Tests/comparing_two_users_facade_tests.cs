using System.Configuration;
using System.Threading.Tasks;
using Shouldly;
using Xunit;

namespace TwitterComparerLibrary.Tests
{
    public class comparing_two_users_facade_tests
    {
        private const string _firstUser = "MDziubiak";
        private const string _secondUser = "mi_checinski";

        private readonly Compare _compare;

        public comparing_two_users_facade_tests()
        {
            var customerKey = ConfigurationManager.AppSettings["CustomerKey"];
            var customerSecret = ConfigurationManager.AppSettings["CustomerSecret"];
            _compare = new Compare(OAuthTwitterToken.GetAsync(customerKey, customerSecret).Result);
        }

        [Fact]
        public async Task compare_two_users_return_not_empty_object()
        {
            var result = await _compare.CompareUsers(_firstUser, _secondUser);

            result.ShouldNotBeNull();
        }

        [Fact]
        public async Task second_call_should_get_object_from_cache()
        {
            var firstResult = await _compare.CompareUsers(_firstUser, _secondUser);
            var secondResult = await _compare.CompareUsers(_firstUser, _secondUser);

            var cached = Cache.Get(_firstUser, _secondUser);

            secondResult.ShouldBe(cached);
        }
    }
}
