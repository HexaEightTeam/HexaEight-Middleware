using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Text;
using System.Security.Cryptography;


namespace HexaEight_Middleware_SampleDemo
{
    public class AuthenticationMiddleware : IAuthenticationService
    {
        static string ComputeSHA512(string s)
        {
            StringBuilder sb = new StringBuilder();
            using (SHA512 sha512 = SHA512.Create())
            {
                byte[] hashValue = sha512.ComputeHash(Encoding.UTF8.GetBytes(s));
                foreach (byte b in hashValue)
                {
                    sb.Append($"{b:X2}");
                }
            }

            return sb.ToString();
        }
        public Task<AuthenticateResult> AuthenticateAsync(HttpContext context, string scheme)
        {
            try
            {
                if (context.User.Identity.IsAuthenticated && context.User.Identity.AuthenticationType == "HexaEight Identity")
                {

                    try
                    {
                        // Check 1: Weed out MITM. This will ensure replaying the same request through some other orign wont work.
                        // Note: This Check wont work for desktop and mobile client apps since orign is expected to be null. Use Check 2 to validate the request
                        if (ComputeSHA512(context.Request.Headers["Origin"].FirstOrDefault().Trim().Replace("https://", "").Replace("http://", "").ToLower()) != context.User.Claims.FirstOrDefault(c => c.Type == "OriginHash").Value)
                        {
                            return Task.FromResult(AuthenticateResult.Fail("UnAuthorized Request - Access Denied"));
                        }
                    }
                    catch { }

                    // Check 2: Validate if the request origniated through list a list of accepted user agents/client applications hash values.
                    // For example if you are expecting api requests from www.client.com, www.thirdparty.com you will add the 512 hash of www.client.com to below list.
                    // For Mobile and Desktop apps, generate the 512 Hash of the executuable and add it below.
                    var listofclienthashes = new string[] { "B5E57824692A50458E29E1E35FCAFE1F55DE18C35DBE0901B5AF887BAC068D08E3B34CEA2B722395D0E4CDFC1292D5E8950894D9FFFB48E834026789CC5F1DCA", "6D14D6852697F68A267C2FD1141AF2BBB88C89A14A512F4E00CC8419C043A4CF681E88D5A064FD68EF1D33175C14BC60975CF4E6CE4BEC101D5287075C823F91", "6D14D6852697F68A267C2FD1141AF2BBB88C89A14A512F4E00CC8419C043A4CF681E88D5A064FD68EF1D33175C14BC60975CF4E6CE4BEC101D5287075C823F91" };

                    if (!listofclienthashes.Any(s => s.Contains(context.User.Claims.FirstOrDefault(c => c.Type == "OriginHash").Value)))
                    {
                        return Task.FromResult(AuthenticateResult.Fail("UnAuthorized User Agent - Access Denied"));
                    }

                    // Check3 : Validate if the request is more than X seconds, if so reject the request.
                    // The below test checks if the request is more than 10 seconds, if so the requst is rejected
                    if (Int64.Parse((context.User.Claims.FirstOrDefault(c => c.Type == "RequestReceivedAt").Value)) - Int64.Parse((context.User.Claims.FirstOrDefault(c => c.Type == "RequestTimeStamp").Value)) > 10)
                    {
                        return Task.FromResult(AuthenticateResult.Fail("Expired Request - Access Denied"));
                    }

                    // TBD -> Check 4 : While Check3 will Prevent most replay attacks, using a  bloom filter based on 'RequestHash' value available in the user claims, can repel all replay attacks

                    AuthenticationTicket at = new AuthenticationTicket(context.User, "HexaEight");
                    return Task.FromResult(AuthenticateResult.Success(at));
                }
                return Task.FromResult(AuthenticateResult.Fail("UnAuthorized"));
            }
            catch
            {
                return Task.FromResult(AuthenticateResult.Fail("UnAuthorized"));
            }


        }

        public Task ChallengeAsync(HttpContext context, string scheme, AuthenticationProperties properties)
        {
            context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
            return Task.CompletedTask;
        }

        public Task ForbidAsync(HttpContext context, string scheme, AuthenticationProperties properties)
        {
            throw new NotImplementedException();
        }

        public Task SignInAsync(HttpContext context, string scheme, ClaimsPrincipal principal, AuthenticationProperties properties)
        {

            return Task.CompletedTask;
        }

        public Task SignOutAsync(HttpContext context, string scheme, AuthenticationProperties properties)
        {
            throw new NotImplementedException();
        }
    }
}

