using Microsoft.AspNetCore.Components.Authorization;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Xml.Linq;

namespace Providers.Contracts
{
    public class CustomAuthenticationStateProvider : AuthenticationStateProvider
    {
        private readonly ClaimsPrincipal anonumous = new ClaimsPrincipal();
        public async override Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            try
            {

                if (string.IsNullOrEmpty(Constants.JWTToken))
                    return await Task.FromResult(new AuthenticationState(anonumous));

                var claims = DecryptToken(Constants.JWTToken);
                if (claims == null)
                    return await Task.FromResult(new AuthenticationState(anonumous));

                var claimsPrincipial = SetClaimPrincipial(claims);
                return await Task.FromResult(new AuthenticationState(claimsPrincipial));
            }
            catch (Exception e)
            {
            }
            return await Task.FromResult(new AuthenticationState(anonumous));
        }

        public CustomUserClaims DecryptToken(string jWTToken)
        {
            if (string.IsNullOrEmpty(jWTToken))
                return new CustomUserClaims();

            var handler = new JwtSecurityTokenHandler();
            var token = handler.ReadJwtToken(jWTToken);

            var name = token.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.Name);
            var nameIdentifier = token.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.NameIdentifier);
            var email = token.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.Email);
            var phoneNumber = token.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.HomePhone);
            return new CustomUserClaims()
            {
                NameIdentifier = nameIdentifier?.Value,
                Name = name?.Value,
                Email = email?.Value,
                MobilePhone = phoneNumber?.Value
            };
        }

        public static ClaimsPrincipal SetClaimPrincipial(CustomUserClaims claims)
        {
            if (string.IsNullOrEmpty(claims.Email))
                return new ClaimsPrincipal();
            return new ClaimsPrincipal(
                new ClaimsIdentity(
                    new List<System.Security.Claims.Claim>
                    {
                        new Claim( type: ClaimTypes.NameIdentifier, value: claims.NameIdentifier),
                        new Claim(type: ClaimTypes.Name, value: claims.Name),
                        new Claim(type: ClaimTypes.Email, value: claims.Email),
                        new Claim(type: ClaimTypes.MobilePhone, value: claims.MobilePhone),
                    }, "JwtAuth")
                );
        }

        public void UpdateAuthenticationState(string jWTToken)
        {
            var claimsPrincipial = new ClaimsPrincipal();
            if (!string.IsNullOrEmpty(jWTToken))
            {
                Constants.JWTToken = jWTToken;
                //await localStorageService.SetToke(jWTToken);
                var claims = DecryptToken(jWTToken);
                claimsPrincipial = SetClaimPrincipial(claims);
            }
            else
            {
                Constants.JWTToken = null;
            }
            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(claimsPrincipial)));
        }
    }
}
