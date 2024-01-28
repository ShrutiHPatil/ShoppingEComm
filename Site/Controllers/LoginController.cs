using ECommSiteApis.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShoppingSite.Models;
using System.Security.Claims;

namespace ShoppingSite.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
         private readonly ApplicationDbContext _context;

        public LoginController(ApplicationDbContext context)
        {
            _context = context;
        }
		[HttpPost("Login")]
		public IActionResult Login([FromBody] LoginModel loginModel)
		{
			try
			{
				var user = _context.Users
					.Where(u => u.Username == loginModel.Username && u.Password == loginModel.Password)
					.Select(u => new
					{
						UserId = u.UserId,
						Username = u.Username,
						FullName = u.FullName
					})
					.FirstOrDefault();

				if (user == null)
				{
					return Unauthorized("Invalid Username or password");
				}

				return Ok(user);
			}
			catch (Exception ex)
			{
				return StatusCode(500, "An error occurred while processing the request.");
			}
		}
		//[HttpPost("Login")]
		//public IActionResult Login([FromBody] LoginModel loginModel)
		//{
		//    try
		//    {

		//        var user = _context.Users.FirstOrDefault(u => u.Username == loginModel.Username && u.Password == loginModel.Password);
		//        if (user == null)
		//        {
		//            return Unauthorized();
		//        }

		//        return Ok(user);
		//    }
		//    catch (Exception ex)
		//    {

		//        return StatusCode(500, "An error occurred while processing the request.");
		//    }
		//}



	}






























        //[HttpPost("login")]
        //public IActionResult Login([FromBody] LoginModel loginModel)
        //{
        //    try
        //    {
        //        var user = _context.Users.FirstOrDefault(u => u.Username == loginModel.Username && u.Password == loginModel.Password);

        //        if (user == null)
        //        {
        //            return Unauthorized();
        //        }

        //        // User is authenticated, set the user identity
        //        var identity = new ClaimsIdentity("custom");
        //        identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()));
        //        identity.AddClaim(new Claim(ClaimTypes.Name, user.Username)); // Assuming username is unique

        //        var principal = new ClaimsPrincipal(identity);
        //        HttpContext.User = principal;

        //        return Ok("Login Successful");
        //    }
        //    catch (Exception ex)
        //    {
        //        // Log the exception
        //        // Return a generic error message for security reasons
        //        return StatusCode(500, "An error occurred while processing the request.");
        //    }
        //}


    }

