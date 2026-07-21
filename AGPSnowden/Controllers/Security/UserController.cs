using AGP.Security.ServiceLayer;
using AGPSnowden.Model;
using AGPSnowden.Model.Auth;
using AGPSnowden.Service.Audits;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AGPSnowden.Controllers.Security
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly AuthService _authService;
        // GET: STable
        public UserController()
        {
            //_authService = new AuthService("","");
        }


        // POST: UserController/Login
    
        /*
        [HttpPost(nameof(Login))]
        public async Task<IActionResult> Login(Login login)
        {
            //Response response = new Response();
            try
            {
                //List<User> audits = new List<User>();

               var user = await _authService.GetUserByLogin(login.Username, login.Password);

                return Ok(user);
            }
            catch(Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }*/
    }
}
