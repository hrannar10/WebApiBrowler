using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Newtonsoft.Json;
using WebApiBrowler.Auth;
using WebApiBrowler.Dtos.Response;
using WebApiBrowler.Models;

namespace WebApiBrowler.Helpers
{
    public class Tokens
    {
        public static async Task<TokenDtoResponse> GenerateJwt(ClaimsIdentity identity, IJwtFactory jwtFactory, string userName, JwtIssuerOptions jwtOptions, JsonSerializerSettings serializerSettings)
        {
            var response = new TokenDtoResponse
            {
                Id = identity.Claims.Single(c => c.Type == "id").Value,
                AuthToken = await jwtFactory.GenerateEncodedToken(userName, identity),
                ExpiresIn = (int)jwtOptions.ValidFor.TotalSeconds
            };
            return response;
        }
    }
}
