using Microsoft.AspNetCore.Mvc;
using dao_library;
using entity_library;
// CAMBIO CLAVE: Añadir estos usings para manejo de archivos y rutas
using Microsoft.AspNetCore.Hosting;
using System.IO;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace apiUser.Controllers
{
    [ApiController]
    [Route("Session")]
    public class SessionController : ControllerBase
    {
        private readonly ILogger<SessionController> _logger;
        private DAOFactory df;
        private readonly IWebHostEnvironment _hostEnvironment;

        public SessionController(ILogger<SessionController> logger, DAOFactory df, IWebHostEnvironment hostEnvironment)
        {
            _logger = logger;
            this.df = df;
            _hostEnvironment = hostEnvironment;
        }

        [HttpPost] 
        public IActionResult Login([FromBody] LoginRequestDTO request) // 👈 Obtenemos datos del cuerpo
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }


                User user = this.df.CreateDAOUser().Login(request.userName);

                if (user == null)
                {
                    return Unauthorized(new { message = "Invalid username or password." });
                }

                bool isPasswordValid = user.IsPasswordValid(request.password);
                if (!isPasswordValid)
                {
                    return Unauthorized(new { message = "Invalid username or password." });
                }

                if (user.IsBanned)
                {
                    Ban ban = this.df.CreateDAOBan().GetBanByUserId(user);
                  
                    return Unauthorized(new
                    {
                        message = "Usuario Baneado.",
                        reason = ban.Reason,
                    });

                }

                LoginResponseDTO response = new LoginResponseDTO
                {
                    id = user.Id,
                    fullName = user.FullName,
                    userName = user.UserName,
                    email = user.Email,
                    urlAvatar = user.GetAvatar()
                };

                ConnectedUsersCounter.Instance.AddUser();
                return Ok(new
                {
                    message = "Login successful",
                    user = response,
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("{id}")]
        public IActionResult Logout([FromRoute] PostLogoutRequestDTO request)
        {
            User user = this.df.CreateDAOUser().GetUser(request.id);
            if (user == null) return NotFound();

            ConnectedUsersCounter.Instance.RemoveUser();
            return Ok(new {
                message = "Sesión cerrada correctamente", 
            });
        }

        [HttpGet]
        public IActionResult GetConnectedUsers()
        {
            int count = ConnectedUsersCounter.Instance.GetCount();
            return Ok(new { connectedUsers = count });
        }

    }
}