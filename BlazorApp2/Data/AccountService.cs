using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using System.Net.Http;
using System.Security.Claims;

namespace BlazorApp2.Data
{
    public class AccountService : IAccountService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AccountService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task LogMeIn()
        {
            var claims = new List<Claim>();
            claims.Add(new Claim(ClaimTypes.Name, "berra@mail.se"));
            claims.Add(new Claim(ClaimTypes.Email, "berra@mail.se"));

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(claimsIdentity);

            //AuthenticationProperties authProperties = new()
            //{
            //    AllowRefresh = true,
            //    IssuedUtc = DateTime.Now,
            //    ExpiresUtc = DateTimeOffset.Now.AddDays(1),
            //    IsPersistent = true,

            //};

            //await _httpContextAccessor.HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

            //var result = await _httpClient.GetJsonAsync<BlazorUser>("api/user/GetUser");

        }
    }
}
