using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace TwitterComparerLibrary
{
    public class OAuthTwitterToken
    {
        public static async Task<string> GenerateAsync(string customerKey, string customerSecret)
        {
            var b64 = Convert.ToBase64String(Encoding.UTF8.GetBytes(
                $"{WebUtility.UrlEncode(customerKey)}:{WebUtility.UrlEncode(customerSecret)}"));
            var req = new HttpRequestMessage(HttpMethod.Post, "https://api.twitter.com/oauth2/token");
            req.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", b64);
            req.Content = new FormUrlEncodedContent(new Dictionary<string, string>() {
                { "grant_type", "client_credentials" }
            });
            var token = "";

            using (var httpClient = new HttpClient())
            {
                using (var res = await httpClient.SendAsync(req))
                {
                    if (res.IsSuccessStatusCode)
                        token = Regex.Match(await res.Content.ReadAsStringAsync(), "\"access_token\":\"([^\"]+)")
                            .Groups[1].Value;
                }
            }
            
            return token;
        }
    }
}