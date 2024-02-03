using Core.Auth;
using Core.Auth.Dtos;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Web;
using Newtonsoft.Json.Serialization;
using Web.Models;
using Common.Exceptions;
using Web.Extensions;

namespace Web.Controllers
{
    public class AuthController : Controller
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        public ActionResult Index()
        {
            return View();
        }

        [Route("/login")]
        public ActionResult Login()
        {
            return View("Login");
        }

        [Route("/register")]
        public ActionResult Register()
        {
            return View("Register");
        }

        [Route("/login")]
        [HttpPost]
        public async Task<ActionResult> SignIn([FromForm] LoginDto form)
        {
            if (!ModelState.IsValid)
            {
                return View("Login");
            }

            try
            {
                var ipAddr = this.GetClientIpAddress();
                var userAgent = this.GetUserAgent();

                form.IpAddress = ipAddr.MapToIPv4().ToString();
                form.UserDevice = userAgent;

                var jwtToken = await _authService.LoginAsync(form);

                var cookieOptions = new CookieOptions();
                cookieOptions.IsEssential = true;
                cookieOptions.Expires = DateTime.Now.AddHours(24);

                Response.Cookies.Append("jwt_token", jwtToken, cookieOptions);

            }
            catch (DomainException ex)
            {
                ViewBag.Success = false;
                ViewBag.Error = ex.Message;
                
                return View("Login");
            }

            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public async Task<ActionResult> SignUp([FromForm] RegisterDto form)
        {
            if (!ModelState.IsValid)
            {
                return View("Register");
            }

             await _authService.RegisterAsync(form);

            return RedirectToAction("Index", "Home");
        }
    }
}
