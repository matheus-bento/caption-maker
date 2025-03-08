using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using CaptionMaker.Data.Model;
using CaptionMaker.Model;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace CaptionMaker.Service.JwtService
{
    public class JwtService
    {
        private readonly IOptions<CaptionMakerOptions> _options;

        public JwtService(IOptions<CaptionMakerOptions> options)
        {
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            this._options = options;
        }

        /// <summary>
        ///     Generates a JWT Token that authenticates the specified user
        /// </summary>
        /// <param name="user">The user that will be authenticated</param>
        public AuthToken Generate(User user)
        {
            var key = Encoding.UTF8.GetBytes(this._options.Value.JwtSecret);

            var credentials = new SigningCredentials(
                new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha256Signature
            );

            DateTime expirationDate = DateTime.Now.AddDays(7);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = this.GetUserClaims(user),
                Expires = expirationDate,
                SigningCredentials = credentials
            };

            var handler = new JwtSecurityTokenHandler();

            var token = handler.CreateToken(tokenDescriptor);

            return new AuthToken
            {
                Token = handler.WriteToken(token),
                ExpiresAt = new DateTimeOffset(expirationDate).ToUnixTimeSeconds()
            };
        }

        /// <summary>
        ///     Generates claims for the specified user
        /// </summary>
        private ClaimsIdentity GetUserClaims(User user)
        {
            var claimsIdentity = new ClaimsIdentity();

            claimsIdentity.AddClaim(new Claim("name", user.Username));

            return claimsIdentity;
        }
    }
}
