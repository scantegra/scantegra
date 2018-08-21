using Microsoft.IdentityModel.Clients.ActiveDirectory;
using Microsoft.Xrm.Tooling.Connector;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scantegra.Core.Functions.Common
{
    public class AuthHook : IOverrideAuthHookWrapper
    {
        // In memory access token cache
        private Dictionary<string, AuthenticationResult> _accessTokens = new Dictionary<string, AuthenticationResult>();

        public void AddAccessToken(Uri orgUri, AuthenticationResult accessToken)
        {
            // Access tokens can be matched by hostname
            _accessTokens[orgUri.Host] = accessToken;
        }

        public string GetAuthToken(Uri connectedUri)
        {
             // Check if an access key for this host thats not expired
             if (_accessTokens.ContainsKey(connectedUri.Host) && _accessTokens[connectedUri.Host].ExpiresOn > DateTime.Now)
            {
                return _accessTokens[connectedUri.Host].AccessToken;
            }
            else
            {
                // Try and refresh the token
                _accessTokens[connectedUri.Host] = GetAccessTokenFromAzureAD(connectedUri);
                return _accessTokens[connectedUri.Host].AccessToken;
            }
        }

        private AuthenticationResult GetAccessTokenFromAzureAD(Uri orgUri)
        {
            // Get the settings from host.json
            string appID = ConfigurationManager.AppSettings["ScantegraAutomationAppID"];
            string secretyKey = ConfigurationManager.AppSettings["ScantegraAutomationAppKey"];

            ClientCredential clientCredential = new ClientCredential(appID, secretyKey);
            AuthenticationParameters authenticationParameters = AuthenticationParameters.CreateFromResourceUrlAsync(orgUri).Result;

            string authorityUrl = authenticationParameters.Authority;
            string resourceUrl = authenticationParameters.Resource;

            AuthenticationContext authenticationContext= new AuthenticationContext(authorityUrl);

            AuthenticationResult authenticationResult = authenticationContext.AcquireTokenAsync(resourceUrl, clientCredential).Result;

            return authenticationResult;
        }


    }
}
