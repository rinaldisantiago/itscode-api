using Microsoft.AspNetCore.Mvc;
using dao_library;
using entity_library;
// CAMBIO CLAVE: Añadir estos usings para manejo de archivos y rutas
using Microsoft.AspNetCore.Hosting;
using System.IO;
using System.Threading.Tasks;

namespace apiUser.Controllers
{
    [ApiController]
    [Route("User")]
    public class UserController : ControllerBase
    {
        private readonly ILogger<UserController> _logger;
        private DAOFactory df;
        private readonly IWebHostEnvironment _hostEnvironment;

        public UserController(ILogger<UserController> logger, DAOFactory df, IWebHostEnvironment hostEnvironment)
        {
            _logger = logger;
            this.df = df;
            _hostEnvironment = hostEnvironment;
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser([FromForm] PostUserRequestDTO request)
        {
            string avatarUrl;
            if (request.image != null && request.image.Length > 0)
            {
                string uploadsFolder = Path.Combine(_hostEnvironment.WebRootPath, "avatars");
                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }
                string uniqueFileName = $"{request.username}_{Guid.NewGuid().ToString()}{Path.GetExtension(request.image.FileName)}";
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await request.image.CopyToAsync(fileStream);
                }
                avatarUrl = $"/avatars/{uniqueFileName}";
            }
            else
            {
                avatarUrl = "/avatars/default.png";
            }
            Image avatar = new Image
            {
                Url = avatarUrl
            };

            Role? role = df.CreateDAORole().GetRoleById(request.roleId ?? (int)RoleEnum.User);
            if (role == null || (role.Id != (int)RoleEnum.User && role.Id != (int)RoleEnum.Admin))
            {
                role = df.CreateDAORole().GetRoleById((int)RoleEnum.User);
            }

            User user = new User
            {
                FullName = request.fullName,
                UserName = request.username,
                Email = request.email,
                Password = new User().SetPassword(request.password),
                Role = role,
                Avatar = avatar
            };

            user.SetPassword(user.Password);
            this.df.CreateDAOUser().CreateUser(user);

            PostUserResponseDTO response = new PostUserResponseDTO
            {
                message = "User created successfully",
            };
            return Ok(response);
        }

        [HttpGet("{id}")]
        public IActionResult getUser([FromRoute] int id)
        {
            User user = this.df.CreateDAOUser().GetUser(id);
            if (user == null)
            {
                return NotFound(new { message = $"User with ID {id} not found." });
            }

            GetUserResponseDTO response = new GetUserResponseDTO
            {
                fullName = user.FullName,
                userName = user.UserName,
                email = user.Email,
                urlAvatar = user.GetAvatar()
            };

            return Ok(response);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, [FromForm] PutUserRequestDTO request)
        {
            User user = this.df.CreateDAOUser().GetUser(id);
            if (user == null)
            {
                return NotFound(new { message = $"User with ID {id} not found." });
            }

            // ... (actualización de fullName, userName, email, password - sin cambios)
            user.FullName = request.fullName;
            user.UserName = request.userName;
            user.Email = request.email;
            if (!string.IsNullOrEmpty(request.password))
            {
                user.SetPassword(request.password);
            }

            // --- LÓGICA DE AVATAR MODIFICADA ---
            // Prioridad 1: Si se sube un archivo de imagen, se procesa y se guarda.
            if (request.image != null && request.image.Length > 0)
            {
                string uploadsFolder = Path.Combine(_hostEnvironment.WebRootPath, "avatars");
                string uniqueFileName = $"{user.UserName}_{Guid.NewGuid()}{Path.GetExtension(request.image.FileName)}";
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await request.image.CopyToAsync(fileStream);
                }
                
                user.Avatar.Url = $"/avatars/{uniqueFileName}";
            }
            // Prioridad 2: Si NO se subió archivo, pero SÍ se proporcionó una URL, se usa esa URL.
            else if (!string.IsNullOrEmpty(request.urlAvatar))
            {
                // Aquí podrías añadir validación para asegurarte de que es una URL válida.
                user.Avatar.Url = request.urlAvatar;
            }
            // Si no se proporciona ni archivo ni URL, el avatar actual no se modifica.

            // ... (guardar cambios y devolver respuesta - sin cambios)
            User updatedUser = this.df.CreateDAOUser().UpdateUser(user);
            PutUserResponseDTO response = new PutUserResponseDTO
            {
                fullName = updatedUser.FullName,
                userName = updatedUser.UserName,
                email = updatedUser.Email,
                urlAvatar = updatedUser.GetAvatar()
            };

            return Ok(response);
        }

        [HttpDelete]
        public IActionResult DeleteUser([FromQuery] DeleteUserRequestDTO request)
        {
            User user = this.df.CreateDAOUser().GetUser(request.id);
            if (user == null) return NotFound();

            this.df.CreateDAOUser().DeleteUser(request.id);

            DeleteUserResponseDTO response = new DeleteUserResponseDTO
            {
                message = "User deleted successfully",
            };


            return Ok(response);


        }


        [HttpPost("{password}/{userName}")] 
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
                    connected = ConnectedUsersCounter.Instance.GetCount()
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("suggestions")]
        public IActionResult Sugerencias([FromQuery] GetSugerenciasResquestDTO request)
        {
            // Obtener sugerencias desde el DAO
            var followingIds = this.df.CreateDAOFollowing().GetFollowedUserIds(request.idUserLogger);
            List<User> sugerencias = this.df.CreateDAOUser()
                .GetSugerencias(request.idUserLogger, request.page, request.pageSize, followingIds);

            // Mapear a DTO de respuesta
            GetSugerenciasResponseDTO response = new GetSugerenciasResponseDTO
            {
                suggestions = sugerencias.Select(u => new UserSuggestionDto
                {
                    id = u.Id,
                    userName = u.UserName,
                    avatar = u.Avatar.Url,
                    isFollowing = followingIds.Contains(u.Id)
                }).ToList()
            };

            return Ok(response);
        }

        [HttpGet("search/{searchTerm}/{idUserLogger}/{pageNumber}/{pageSize}")]
        public IActionResult GetUsersBySearch([FromRoute] SearchUsersRequestDTO request)
        {
            try
            {
                int pageNumber = request.pageNumber <= 0 ? 1 : request.pageNumber;
                int pageSize = request.pageSize <= 0 ? 1 : request.pageSize;

                var followingIds = this.df.CreateDAOFollowing().GetFollowedUserIds(request.idUserLogger);

                List<User> users = this.df.CreateDAOUser().SearchUsers(request.searchTerm, request.idUserLogger, request.pageNumber, request.pageSize);
                var allUsers = users.Select(user => new UserSuggestionDto
                {
                    id = user.Id,
                    avatar = user.GetAvatar(),
                    userName = user.UserName,
                    isFollowing = followingIds.Contains(user.Id)
                })
                .ToList();

                var response = new SearchUsersResponseDTO
                {
                    users = allUsers
                };

                return Ok(response);

            }
            catch(Exception ex)
            {
                return StatusCode(500, new
                {
                    message = "An unexpected error occurred.",
                    error = ex.Message
                });
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
                connected = ConnectedUsersCounter.Instance.GetCount() //TODO: ver sei necesita un dto response
            });
        }

    }
}
