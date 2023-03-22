using DAL;
using DAL.Entities;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Services.Authentication
{
    public class Authentication
    {
        UnitOfWork uow = null;

        public Authentication(UnitOfWork uow)
        {
            this.uow = uow;
        }

        const string mySecret = "asdv234234^&%&^%&^hjsdfb2%%%";
        const string myIssuer = "http://schischoissuer.com";
        const string myAudience = "http://schischoaudience.com";
        SymmetricSecurityKey mySecurityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(mySecret));

        public async Task<AuthenticationInformation> Authenticate(User usr)
        {


            if (usr != null)
            {
                AuthenticationInformation info = new AuthenticationInformation();

                DateTime expires = DateTime.UtcNow.AddDays(7);

                info.ExpirationDate = new DateTimeOffset(expires).ToUnixTimeSeconds();

                var tokenHandler = new JwtSecurityTokenHandler();
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new Claim[]
                    {
                        new Claim(ClaimTypes.NameIdentifier, usr.ID.ToString()),
                        new Claim(ClaimTypes.GivenName, usr.Lastname + " " + usr.Firstname),
                        new Claim(ClaimTypes.Email, usr.Email)
                    }),
                    Expires = expires,
                    Issuer = myIssuer,
                    Audience = myAudience,
                    SigningCredentials = new SigningCredentials(mySecurityKey, SecurityAlgorithms.HmacSha256Signature)
                };

                var token = tokenHandler.CreateToken(tokenDescriptor);
                info.Token = tokenHandler.WriteToken(token);

                return info;
            }

            return null;
        }



        public async Task<bool> ValidateCurrentToken(string token)
        {


            var tokenHandler = new JwtSecurityTokenHandler();
            try
            {
                var val = tokenHandler.ValidateToken(token, ValidationParams, out SecurityToken validatedToken);
            }
            catch
            {
                return false;
            }
            return true;
        }

        public static TokenValidationParameters ValidationParams
        {
            get
            {
                return new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidIssuer = myIssuer,
                    ValidAudience = myAudience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(mySecret))
                };
            }
        }

        public string GetEmailByToken(string token)
        {

            var tokenHandler = new JwtSecurityTokenHandler();
            try
            {
                string finaltoken = token;

                if (token.StartsWith("Bearer "))
                {
                    finaltoken = token.Replace("Bearer ", "");
                }

                var val = tokenHandler.ValidateToken(finaltoken, ValidationParams, out SecurityToken validatedToken);

                if (val.HasClaim(x => x.Type == ClaimTypes.Email))
                {
                    Claim claim = val.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email);

                    if (claim != null && !string.IsNullOrEmpty(claim.Value))
                    {
                        return claim.Value;
                    }
                }
            }
            catch
            {
                return null;
            }
            return null;
        }

        public string GetClaim(string token, string claimType)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var securityToken = tokenHandler.ReadToken(token) as JwtSecurityToken;

            var stringClaimValue = securityToken.Claims.First(claim => claim.Type == claimType).Value;
            return stringClaimValue;
        }
    }
}
