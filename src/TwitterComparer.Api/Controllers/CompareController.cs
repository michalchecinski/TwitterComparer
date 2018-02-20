using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TwitterComparerLibrary;

namespace TwitterComparer.Api.Controllers
{
    [Produces("application/json")]
    [Route("api/Compare")]
    public class CompareController : Controller
    {
        private readonly ICompare _compare;

        public CompareController(ICompare compare)
        {
            _compare = compare;
        }

        [HttpGet]
        public IEnumerable<string> Get(string firstUsername, string secondUserName)
        {
            return _compare.CompareUsers( , firstUsername, secondUserName);
        }
    }
}