
/*
using PayPal.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Utils
{
    public static class PayPalConfiguration
    {
        static PayPalConfiguration()
        {
            
        }
        public static Dictionary<string, string> GetConfig(string mode)
        {
            return new Dictionary<string, string>()
            {
                {"mode", mode }
            };
        }
        private static string GetAccessToken(string ClientId, 
                                            string Secret,
                                            string mode)
        {
            string accessToken = new OAuthTokenCredential(ClientId
                                                        , Secret, new Dictionary<string, string>()
            {
                {"mode", mode }
            }).GetAccessToken();
            return accessToken;
        }
        //public static APIContext GetAPIContext(string ClientId, string Secret, string mode)
        {
        //    APIContext apiContext = new APIContext(GetAccessToken(ClientId, Secret, mode));
        //    apiContext.Config = GetConfig(mode);
         //   return apiContext;
        }

    }
}
*/

