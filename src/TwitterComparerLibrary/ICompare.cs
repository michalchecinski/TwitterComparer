using System.Threading.Tasks;
using TwitterComparerLibrary.Model;

namespace TwitterComparerLibrary
{
    public interface ICompare
    {
        Task<ComparedUsersDto> CompareUsers(string firstUserName, string secondUserName, string token);
    }
}