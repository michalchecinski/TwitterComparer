using System.Collections.Generic;
using System.Threading.Tasks;

namespace TwitterComparerLibrary
{
    public interface ICompareUsersFriends
    {
        Task<int> GetCommonFriendsNumberAsync(string firstUserName, string secondUserName);
        Task<List<User>> GetCommonFriendsListAsync(string firstUserName, string secondUserName);
    }
}