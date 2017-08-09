using System.Collections.Generic;
using System.Threading.Tasks;

namespace TwitterComparerLibrary
{
    public interface ICompareUsersFollowers
    {
        Task<int> CommonFollowersNumberAsync(string firstUserName, string secondUserName);
        Task<List<User>> GetCommonFollowersListAsync(string firstUserName, string secondUserName);
    }
}