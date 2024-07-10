using Microsoft.IdentityModel.Tokens;
using MyApp.Infrastructure.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MyApp.Web.Helper
{
    public class TokenProvider
    {
        private BloggingContext _context;
        public TokenProvider(BloggingContext context)
        {
            _context = context;
        }
        public string LoginUser(string username, string password, bool remember = false)
        {
            var user = _context.Users.FirstOrDefault(p => p.UserName == username && PasswordHelper.HashPassword(password) == p.Password);

            var userClaims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Role, "user"),
                new Claim(ClaimTypes.GivenName, user.FullName),
                new Claim(ClaimTypes.Email, user.Email),
            };
            var token = GetToken(userClaims);
            return token;
        }
        private string GetToken(List<Claim> userClaims)
        {
            var key = Encoding.ASCII.GetBytes("a1b2c3-tcn");
            var JWToken = new JwtSecurityToken(
                issuer: "https://localhost:5001/",
                audience: "https://localhost:5001/",
                claims: userClaims,
                notBefore: new DateTimeOffset(DateTime.Now).DateTime,
                expires: new DateTimeOffset(DateTime.Now.AddDays(1)).DateTime,
                signingCredentials: new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                );
            var token = new JwtSecurityTokenHandler().WriteToken(JWToken);
            return token;
        }
    }
}
