using Microsoft.AspNetCore.Mvc;
using dao_library;
using entity_library;


namespace apiUser.Controllers
{
    [ApiController]
    [Route("User")]
    public class UserController : ControllerBase
    {
        private readonly ILogger<UserController> _logger;
        private DAOFactory df;

        public UserController(ILogger<UserController> logger, DAOFactory df)
        {
            _logger = logger;
            this.df = df;
        }


        [HttpPost]
// 🚨 CAMBIO CLAVE: Usamos [FromForm] para leer FormData (campos de texto + archivo)
        public IActionResult CreateUser([FromForm] PostUserRequestDTO request)
        {
            // 1. INICIALIZACIÓN: Definir la URL del avatar
            string avatarUrl;

            if (request.Image != null)
            {
                // 🚨 LÓGICA DE SUBIDA DE ARCHIVO (Delegar la responsabilidad)
                
                // Esta es la parte que tienes que implementar usando tu capa DAO/Service.
                // Aquí se llamaría a un servicio: var uploadedResult = _fileService.Upload(request.Image);
                
                // POR AHORA, para probar el flujo completo: simulamos el guardado
                // y le asignamos una URL (EJEMPLO, DEBES REEMPLAZAR ESTO)
                avatarUrl = $"http://localhost:5052/avatars/{request.Username}_{DateTime.Now.Ticks}.jpg";
                
                // Si necesitas guardar el archivo físicamente, el código iría aquí o en un servicio.
            }
            else
            {
                // Si no hay archivo, usamos la URL por defecto o la que venga en el DTO
                avatarUrl = request.URLAvatar ?? "https://example.com/default.jpg";
            }

            // 2. CREACIÓN DEL OBJETO AVATAR CON LA URL DEFINIDA
            Image avatar = new Image
            {
                Url = avatarUrl 
            };

            // 3. Lógica de Rol (SIN CAMBIOS)
            Role? role = df.CreateDAORole().GetRoleById(request.RoleId ?? (int)RoleEnum.User);

            if (role == null || (role.Id != (int)RoleEnum.User && role.Id != (int)RoleEnum.Admin))
            {
                role = df.CreateDAORole().GetRoleById((int)RoleEnum.User);
            }
            
            // 4. CREACIÓN DEL USUARIO (SIN CAMBIOS, usa los datos del 'request')
            User user = new User
            {
                FullName = request.FullName,
                UserName = request.Username,
                Email = request.Email,
                Password = request.Password,
                Role = role,
                Avatar = avatar // Usamos el objeto Avatar con la URL
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
        public IActionResult getUser([FromQuery] GetUserRequestDTO request)
        {
            User user = this.df.CreateDAOUser().GetUser(request.Id);
            if (user == null) return null;

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


        [HttpPost("Login")] // 👈 CAMBIO CLAVE: Cambiamos a POST y le damos una ruta específica
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
        public IActionResult GetUsersBySearch([FromRoute] GetAllUsersRequestDTO request)
        {
            try
            {
                int pageNumber = request.pageNumber <= 0 ? 1 : request.pageNumber;
                int pageSize = request.pageSize <= 0 ? 1 : request.pageSize;

                var followingIds = this.df.CreateDAOFollowing().GetFollowedUserIds(request.idUserLogger);

                List<User> users = this.df.CreateDAOUser().SearchUsers(request.searchTerm, request.idUserLogger, request.pageNumber, request.pageSize);
                var allUsers = users.Select(user => new GetUsersResponseDTO
                {
                    id = user.Id,
                    userAvatar = user.GetAvatar(),
                    userName = user.UserName,
                    isFollowing = followingIds.Contains(user.Id)
                })
                .ToList();

                var response = new GetAllUsersResponseDTO
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