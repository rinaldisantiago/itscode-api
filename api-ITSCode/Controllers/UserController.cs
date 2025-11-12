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
        // CAMBIO CLAVE: Añadir un campo para el entorno web
        private readonly IWebHostEnvironment _hostEnvironment;

        // CAMBIO CLAVE: Inyectar IWebHostEnvironment en el constructor
        public UserController(ILogger<UserController> logger, DAOFactory df, IWebHostEnvironment hostEnvironment)
        {
            _logger = logger;
            this.df = df;
            _hostEnvironment = hostEnvironment; // Guardar la instancia
        }

        [HttpPost]
        // CAMBIO CLAVE: Hacer el método asíncrono
        public async Task<IActionResult> CreateUser([FromForm] PostUserRequestDTO request)
        {
            string avatarUrl;

            // CAMBIO CLAVE: LÓGICA REAL PARA GUARDAR EL ARCHIVO
            if (request.Image != null && request.Image.Length > 0)
            {
                // 1. Definir la carpeta de destino (dentro de wwwroot para acceso público)
                string uploadsFolder = Path.Combine(_hostEnvironment.WebRootPath, "avatars");
                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                // 2. Crear un nombre de archivo único para evitar sobreescrituras
                string uniqueFileName = $"{request.Username}_{Guid.NewGuid().ToString()}{Path.GetExtension(request.Image.FileName)}";
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                // 3. Guardar el archivo en el disco de forma asíncrona
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await request.Image.CopyToAsync(fileStream);
                }

                // 4. Generar la URL pública relativa que se guardará en la base de datos
                avatarUrl = $"/avatars/{uniqueFileName}";
            }
            else
            {
                // Si no se sube imagen, usamos una por defecto
                avatarUrl = "/avatars/default.png"; // Asegúrate de tener una imagen default.png en wwwroot/avatars
            }

            // --- El resto de tu lógica de creación de usuario permanece igual ---
            Image avatar = new Image
            {
                Url = avatarUrl
            };

            Role? role = df.CreateDAORole().GetRoleById(request.RoleId ?? (int)RoleEnum.User);
            if (role == null || (role.Id != (int)RoleEnum.User && role.Id != (int)RoleEnum.Admin))
            {
                role = df.CreateDAORole().GetRoleById((int)RoleEnum.User);
            }

            User user = new User
            {
                FullName = request.FullName,
                UserName = request.Username,
                Email = request.Email,
                Password = request.Password,
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
        public IActionResult getUser(int id) // <-- ¡Solucionado! Ahora toma el 'id' de la URL
        {
            User user = this.df.CreateDAOUser().GetUser(id); // Usamos el 'id' directamente
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

        [HttpPut]
        public IActionResult UpdateUser(int id, [FromBody] PutUserRequestDTO request)
        {
            User user = this.df.CreateDAOUser().GetUser(id);
            if (user == null) return NotFound();
            user.FullName = request.fullName;
            user.UserName = request.userName;
            user.Email = request.email;
            user.encript(request.password);
            user.Avatar.Url = request.urlAvatar;


            User updateUser = this.df.CreateDAOUser().UpdateUser(user);

            PutUserResponseDTO response = new PutUserResponseDTO
            {
                fullName = updateUser.FullName,
                userName = updateUser.UserName,
                email = updateUser.Email,
                urlAvatar = updateUser.GetAvatar()
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
                Message = "User deleted successfully",
            };


            return Ok(response);


        }


        [HttpPost("Login")] 
        public IActionResult Login([FromBody] LoginRequestDTO request) // 👈 Obtenemos datos del cuerpo
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                // Aquí el request.userName y request.password vienen del cuerpo (body) de la solicitud HTTP

                string encryptedPassword = new User().encript(request.password);

                User user = this.df.CreateDAOUser().Login(request.userName, encryptedPassword);

                if (user == null)
                {
                    return Unauthorized(new { message = "Invalid username or password." });
                }

                if (user.IsBanned)
                {
                    Ban ban = this.df.CreateDAOBan().GetBanByUserId(user.Id);
                  
                    return Unauthorized(new
                    {
                        message = "Usuario Baneado.",
                        reason = ban.Reason,
                    });

                }

                LoginResponseDTO response = new LoginResponseDTO
                {
                    Id = user.Id,
                    FullName = user.FullName,
                    UserName = user.UserName,
                    Email = user.Email,
                    UrlAvatar = user.GetAvatar()
                };

                return Ok(new
                {
                    message = "Login successful",
                    user = response
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("Sugerencias")]
        public IActionResult Sugerencias([FromQuery] GetSugerenciasResquestDTO request)
        {
            // Obtener sugerencias desde el DAO
            var followingIds = this.df.CreateDAOFollowing().GetFollowedUserIds(request.idUserLogger);
            List<User> sugerencias = this.df.CreateDAOUser()
                .GetSugerencias(request.idUserLogger, request.page, request.pageSize, followingIds);

            // Mapear a DTO de respuesta
            GetSugerenciasResponseDTO response = new GetSugerenciasResponseDTO
            {
                Sugerencias = sugerencias.Select(u => new UserSuggestionDto
                {
                    id = u.Id,
                    userName = u.UserName,
                    avatar = u.Avatar.Url,
                    isFollowing = followingIds.Contains(u.Id)
                }).ToList()
            };

            return Ok(response);
        }

        [HttpGet("{searchTerm}/{idUserLogger}/{pageNumber}/{pageSize}")]
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

    }
}