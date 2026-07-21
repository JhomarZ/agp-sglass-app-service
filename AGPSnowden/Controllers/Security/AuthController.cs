using AGP.Security.DataAccessLayer;
using AGP.Security.ServiceLayer;
using AGP.Snowden.DataAccessLayer;
using AGP.Snowden.ServiceLayer.RRHH;
using AGPSnowden.Controllers.RRHH;
using AGPSnowden.Model.Auth;
using Microsoft.AspNetCore.Mvc;

namespace AGPSnowden.Controllers.Security
{

    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly AuthService _authService;
        private readonly PersonalService _personalService;
        private readonly AreaService _areaService;
        private readonly int _expreInDays;
        private readonly int _expireInMinutes;
        public AuthController(IConfiguration configuration)
        {
            _authService = new AuthService(configuration);
            _areaService = new AreaService();
            _personalService= new PersonalService();

            var jwtToken = configuration.GetSection("TokenSettings");
            _expreInDays = jwtToken !=null? Convert.ToInt32(jwtToken["ExpiresInDays"]):15;
            _expireInMinutes = Convert.ToInt32(jwtToken["ExpiresInMinutes"]);
        }

        [HttpPost("login")]
        public IActionResult Login(LoginRequest request)
        {
            try
            {

                // Verificar las credenciales del usuario (ejemplo simplificado)

                var user = _authService.GetUserByLogin(request.Username, request.Password);

                var accessTokenExpires = DateTime.UtcNow.AddMinutes(_expireInMinutes);
                var refreshTokenExpires = DateTime.UtcNow.AddDays(_expreInDays);

                var accessToken = _authService.GenerateAccessToken(user.Id, accessTokenExpires);
                var refreshToken = _authService.GenerateRefreshToken();


                // Guardar el refresh token en la base de datos o en un almacenamiento seguro
                AGP.Security.DataAccessLayer.Token token = new AGP.Security.DataAccessLayer.Token();
                token.AccessToken = accessToken;
                token.UserId = user.Id;
                token.RefreshTokenExpirationDate = refreshTokenExpires;
                token.RefreshToken = refreshToken;
                token.AccessTokenExpirationDate = accessTokenExpires;

                //token = _authService.SaveRefreshToken(token);
                user.Token = token;
                user.Area = _areaService.GetArea(user.AreaId);
                //token.User = user;
                //user.Tokens.Add(token);

                return Ok(user);
                
            }
            catch(Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

            return Unauthorized();
        }

        [HttpPost("login-rrhh")]
        public async Task<IActionResult> LoginRRHH(LoginRequest request)
        {
            try
            {

                // Verificar las credenciales del usuario (ejemplo simplificado)

                Personal personal =await  _personalService.GetOneByDocumentNumber(request.Username);

                if (personal == null) 
                    {
                        throw new Exception("Error, usuario no existe");
                    } 


                /*
                var accessTokenExpires = DateTime.UtcNow.AddMinutes(_expireInMinutes);
                var refreshTokenExpires = DateTime.UtcNow.AddDays(_expreInDays);

                var accessToken = _authService.GenerateAccessToken(personal.Id, accessTokenExpires);
                var refreshToken = _authService.GenerateRefreshToken();


                // Guardar el refresh token en la base de datos o en un almacenamiento seguro
                AGP.Security.DataAccessLayer.Token token = new AGP.Security.DataAccessLayer.Token();
                token.AccessToken = accessToken;
                token.UserId = personal.Id;
                token.RefreshTokenExpirationDate = refreshTokenExpires;
                token.RefreshToken = refreshToken;
                token.AccessTokenExpirationDate = accessTokenExpires;
                token.personal = personal; */


                return Ok(personal);

            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

            return Unauthorized();
        }


        [HttpPost("refresh-token")]
        public IActionResult RefreshToken(RefreshTokenRequest request)
        {
            var principal = _authService.ValidateToken(request.RefreshToken);

            // Verificar que el refresh token es válido

            var accessTokenExpires = DateTime.UtcNow.AddMinutes(_expireInMinutes);
            var accessToken = _authService.GenerateAccessToken(1, accessTokenExpires);

            return Ok(new RefreshTokenResponse
            {
                AccessToken = accessToken,
                AccessTokenExpirationDate = accessTokenExpires
            });
        }
    }
}
