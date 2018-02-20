using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TwitterComparerLibrary;
using Microsoft.Extensions.Configuration;
using TwitterComparerLibrary.Model;
using System.Web;
using Microsoft.AspNetCore.Server.Kestrel.Core.Internal.Http;

namespace TwitterComparer.Api.Controllers
{
    [Produces("application/json")]
    [Route("api/Compare")]
    public class CompareController : Controller
    {
        private readonly ICompare _compare;
        public IConfiguration Configuration { get; set; }
        private readonly ITokenProvider _tokenProvider;

        private static string _token;

        public CompareController(ICompare compare, ITokenProvider tokenProvider, IConfiguration config)
        {
            _compare = compare;
            _tokenProvider = tokenProvider;
            Configuration = config;
        }

        [HttpGet]
        public async Task<IActionResult> Get(string firstUser, string secondUser)
        {
            if (firstUser == null)
            {
                return StatusCode((int)HttpStatusCode.BadRequest, "You have not include firstUser parameter in the URI.");
            }

            if (secondUser == null)
            {
                return StatusCode((int)HttpStatusCode.BadRequest, "You have not include secondUser parameter in the URI.");
            }

            var customerKey = Configuration["TwitterCustomerKey"];
            var customerSecret = Configuration["TwitterCustomerSecret"];

            _token = await _tokenProvider.GetAsync(customerKey, customerSecret);

            CompareUsersResult result = null;

            try
            {
                result = await _compare.CompareUsers(_token, firstUser, secondUser);
            }
            catch (WebException e)
            {
                if (e.Message.Contains("Content not found"))
                {
                    return StatusCode((int)HttpStatusCode.NotFound, e.Message+" on Twitter.");
                }
                if (e.Status == WebExceptionStatus.ReceiveFailure)
                {
                    return StatusCode((int) HttpStatusCode.RequestTimeout, e.Message);
                }
            }

            return Ok(Json(result));
        }
    }
}