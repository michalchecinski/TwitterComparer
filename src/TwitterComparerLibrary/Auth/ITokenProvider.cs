using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwitterComparerLibrary
{
    public interface ITokenProvider
    {
        Task<string> GetAsync(string customerKey, string customerSecret);
    }
}
