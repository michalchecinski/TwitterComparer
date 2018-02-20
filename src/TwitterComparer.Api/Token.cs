using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TwitterComparerLibrary;

namespace TwitterComparer.Api
{
    public class Token
    {
        private readonly ITokenProvider _tokenProvider;
        public Token(ITokenProvider tokenProvider)
        {
            _tokenProvider = tokenProvider;
        }
    }
}
