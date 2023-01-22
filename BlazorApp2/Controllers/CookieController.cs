using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BlazorApp2.Controllers
{
    [ApiController]
    public class CookieController : ControllerBase
    {
        [HttpGet("login2")]
        public async Task<ActionResult> Login2()
        {
            var claims = new List<Claim>();
            claims.Add(new Claim(ClaimTypes.Name, "berra@mail.se"));
            claims.Add(new Claim(ClaimTypes.Email, "berra@mail.se"));

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(claimsIdentity);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
            return Redirect("/");
        }

        private static readonly AuthenticationProperties COOKIE_EXPIRES = new AuthenticationProperties()
        {
            ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(10),
            IsPersistent = true,
        };

        [HttpPost]
        [Route("api/auth/signin")]
        public async Task<ActionResult> SignInPost()
        {
            var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Email, "berra@mail.se"),
            new Claim(ClaimTypes.Name,  "berra@mail.se"),
            new Claim(ClaimTypes.Role,  "Administrator"),
        };

            var claimsIdentity = new ClaimsIdentity(claims,
                                                    CookieAuthenticationDefaults.AuthenticationScheme);
            var authProperties = COOKIE_EXPIRES;

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                                          new ClaimsPrincipal(claimsIdentity),
                                          authProperties);

            //return Redirect("/");
            return this.Ok();
        }

        [HttpPost]
        [Route("api/auth/signout")]
        public async Task<ActionResult> SignOutPost()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return this.Ok();
        }
    }

    public class SigninData
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
