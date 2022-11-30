using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using ModelBindingTask.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using ModelBindingTask.Data;

namespace ModelBindingTask.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;

        private readonly ApplicationDbContext _DbContext;

        public AuthController(UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            IConfiguration configuration, ApplicationDbContext applicationDbContext)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
            _DbContext = applicationDbContext;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] User model)
        {
            //var user = await _userManager.FindByNameAsync(model.Username);
            //var user = await _userManager;
            var user = _userManager.Users.Where(x=>x.UserName==model.Username).FirstOrDefault();
            //if (user != null && await _userManager.CheckPasswordAsync(user, model.UserPassword))
            if (user != null && user.UserName == model.Username)
                {
                var userRoles = await _userManager.GetRolesAsync(user);

                var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.UserName),
                    //new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                };

                foreach (var userRole in userRoles)
                {
                    authClaims.Add(new Claim(ClaimTypes.Role, userRole));
                }

                var token = LoginDefaultJwt(authClaims);

                //return Ok(new
                //{
                //    token = new JwtSecurityTokenHandler().WriteToken((SecurityToken)token)
                //    expiration = token.ValidTo
                //});
                return Ok(token);
            }
            return Unauthorized();
        }





        //[HttpPost("loginDefaultJwt")]
        //public IActionResult LoginDefaultJwt([FromBody] User user)
        public IActionResult LoginDefaultJwt(List<Claim> authClaims)
        {
            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("superSecretKey@1"));
            var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

            var tokenOptions = new JwtSecurityToken(
                issuer: "https://localhost:44363/",
                audience: "https://localhost:44363/",
                 claims: authClaims,
                //claims: new List<Claim>() { new Claim(ClaimTypes.Name, user.Username ?? string.Empty) },
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: signinCredentials
            );

            var tokenString = new JwtSecurityTokenHandler().WriteToken(tokenOptions);

            return Ok(new { Token = tokenString });
        }


        [HttpPost("loginSecondJwt")]
        public IActionResult LoginSecondJwt([FromBody] User user)
        {
            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("superSecretKey@2"));
            var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

            var tokenOptions = new JwtSecurityToken(
                issuer: "https://localhost:44363/",
                audience: "https://localhost:44363/",
                claims: new List<Claim>() { new Claim(ClaimTypes.Name, user.Username ?? string.Empty) },
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: signinCredentials
            );

            var tokenString = new JwtSecurityTokenHandler().WriteToken(tokenOptions);

            return Ok(new { Token = tokenString });
        }

        [HttpPost("loginCookie")]
        public async Task<IActionResult> LoginCookie([FromBody] User user)
        {
            var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme, ClaimTypes.Name, ClaimTypes.Role);
            identity.AddClaim(new Claim(ClaimTypes.Name, user.Username ?? string.Empty));
            var principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                principal,
                new AuthenticationProperties
                {
                    IsPersistent = true,
                    AllowRefresh = true,
                    ExpiresUtc = DateTime.UtcNow.AddDays(1)
                });

            return Ok();
        }


        [HttpGet("unauthorized")]
        public IActionResult GetUnauthorized()
        {
            return Unauthorized();
        }

        [HttpGet("forbidden")]
        public IActionResult GetForbidden()
        {
            return Forbid();
        }
    }
}
