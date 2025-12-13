using Microsoft.AspNetCore.Mvc;
using dao_library;
using entity_library;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.ComponentModel.DataAnnotations;

namespace apiUser.Controllers
{
    [ApiController]
    [Route("Session")]
    public class SessionController : ControllerBase
    {
        private readonly ILogger<SessionController> _logger;
        private DAOFactory df;
        private readonly IWebHostEnvironment _hostEnvironment;

        private void ValidateRequest(LoginRequestDTO request)
        {
            if (string.IsNullOrWhiteSpace(request.userName) || string.IsNullOrWhiteSpace(request.password))
                throw new ValidationException("Usuario y/o contraseña son obligatorios.");
        }

        private User AuthenticateUser(LoginRequestDTO request)
        {
            User? user = this.df.CreateDAOUser().Login(request.userName);

            if (user is null || !user.IsPasswordValid(request.password))
                throw new UnauthorizedAccessException("Usuario o contraseña inválidos.");

            return user;
        }

        private void CheckAccessRules(User user, LoginRequestDTO request)
        {
            CheckUserIsBanned(user);
            CheckUserRole(user, request);
        }

        private void CheckUserIsBanned(User user)
        {
            if (user.IsBanned)
            {
                Ban? ban = this.df.CreateDAOBan().GetBanByUserId(user);
                throw new UnauthorizedAccessException($"Usuario Baneado. Razón: {ban.Reason}.");
            }
        }

        private void CheckUserRole(User user, LoginRequestDTO request)
        {
            bool requiresAdmin = request.isLoginDashboard;
            bool isNotAdmin = user.Role == null || !user.GetRole().Equals((int)RoleEnum.Admin);

            if (requiresAdmin && isNotAdmin)
            {
                throw new UnauthorizedAccessException("Acceso denegado. Se requieren permisos de administrador.");
            }
        }

        private LoginResponseDTO ProcessSuccessfulLogin(User user)
        {
            Singleton.GetInstance().AddUser();

            return new LoginResponseDTO
            {
                id = user.Id,
                fullName = user.FullName,
                userName = user.UserName,
                email = user.Email,
                urlAvatar = user.GetAvatar()
            };
        }

        public SessionController(ILogger<SessionController> logger, DAOFactory df, IWebHostEnvironment hostEnvironment)
        {
            _logger = logger;
            this.df = df;
            _hostEnvironment = hostEnvironment;
        }

        [HttpPost]
        public IActionResult Login([FromBody] LoginRequestDTO request)
        {
            try
            {
                ValidateRequest(request);
                User user = AuthenticateUser(request);
                CheckAccessRules(user, request);
                var response = ProcessSuccessfulLogin(user);

                return Ok(new 
                { 
                    message = "Login exitoso", 
                    user = response 
                });
            }

            catch (ValidationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }

            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { message = ex.Message });
            }

            catch (Exception ex)
            {
                return StatusCode(500, new
                    {
                        message = "Error interno del servidor.",
                        error = ex.Message
                    });
            }
        }

        [HttpPost("{id}")]
        public IActionResult Logout([FromRoute] PostLogoutRequestDTO request)
        {
            try
            {
                User? user = this.df.CreateDAOUser().GetUser(request.id);
                if (user is null) return NotFound();

                Singleton.GetInstance().RemoveUser();
                return Ok(new {
                    message = "Sesión cerrada correctamente", 
                });
            }

            catch(Exception ex)
            {
                return StatusCode(500, new
                {
                    message = "Error interno del servidor.",
                    error = ex.Message
                });
            }
        }

        [HttpGet]
        public IActionResult GetConnectedUsers()
        {
            try
            {
                int count = Singleton.GetInstance().GetCount();
                return Ok(new { connectedUsers = count });
            }

            catch(Exception ex)
            {
                return StatusCode(500, new
                {
                    message = "Error interno del servidor.",
                    error = ex.Message
                });
            }
        }

    }
}