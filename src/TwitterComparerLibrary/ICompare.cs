using System.Threading.Tasks;
using TwitterComparerLibrary.Model;

namespace TwitterComparerLibrary
{
    public interface ICompare
    {
        Task<CompareUsersResult> CompareUsers(string firstUserName, string secondUserName);
    }
}