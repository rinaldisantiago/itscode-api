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
            //Validar todas las entradas
            if(String.IsNullOrWhiteSpace(request.fullName) ||
               String.IsNullOrWhiteSpace(request.username) ||
               String.IsNullOrWhiteSpace(request.email) ||
               String.IsNullOrWhiteSpace(request.password))
            {
                return BadRequest(new { message = "Algunos campos son obligatorios" } );
            }

            //Validar formato fullname
            string pattern = @"^(?=.{1,50}$)[a-zA-ZÀ-ÿ]+( [a-zA-ZÀ-ÿ]+)+$";
            if (!Regex.IsMatch(request.fullName, pattern, RegexOptions.None))
            {
                return BadRequest(new { message = "El nombre completo no cumple con el formato requerido" });
            }

            //Validar formato username
            pattern = @"^(?=.{5,25}$)[a-zA-Z0-9_]+$";
            if (!Regex.IsMatch(request.username, pattern, RegexOptions.None))
            {
                return BadRequest(new { message = "El nombre de usuario no cumple con el formato requerido" });
            }

            //Validar formato email
            pattern = @"^[^\s@]+@[^\s@]+\.[^\s@]+$";
            if (!Regex.IsMatch(request.email, pattern, RegexOptions.IgnoreCase))
            {
                return BadRequest(new { message = "El correo electrónico no tiene el formato requerido" });
            }

            //Validar formato password
            pattern = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[\W_]).{8,}$";
            if (!Regex.IsMatch(request.password, pattern, RegexOptions.None))
            {
                return BadRequest(new { message = "La contraseña no cumple con los requisitos mínimos" });
            }

            //Validar username existente
            bool userExists = this.df.CreateDAOUser().GetUserByUsername(request.username);
            if(userExists)
            {
                return BadRequest(new { message = "El nombre de usuario ya existe" });
            }

            //Validar email existente
            bool emailExists = this.df.CreateDAOUser().GetUserByEmail(request.email);
            if (emailExists)
            {
                return BadRequest(new { message = "El email ya existe" });
            }     

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
                FullName = request.fullName.Trim(),
                UserName = request.username.Trim(),
                Email = request.email.Trim().ToLower(),
                Password = new User().SetPassword(request.password).Trim(),
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

        [HttpPut]
        public async Task<IActionResult> UpdateUser([FromForm] PutUserRequestDTO request)
        {
            User user = this.df.CreateDAOUser().GetUser(request.id);
            if (user == null)
            {
                return NotFound(new { message = $"User with ID {request.id} not found." });
            }

            if (!string.IsNullOrEmpty(request.fullName))
            {
                string newFullName = request.fullName.Trim();
                string patternFullName = @"^(?=.{1,50}$)[a-zA-ZÀ-ÿ]+( [a-zA-ZÀ-ÿ]+)+$";

                if (!Regex.IsMatch(newFullName, patternFullName, RegexOptions.None))
                {
                    return BadRequest(new { message = "El nombre completo no cumple con el formato requerido." });
                }

                user.FullName = newFullName;
            }

            if (!string.IsNullOrEmpty(request.userName))
            {
                string newUserName = request.userName.Trim();

                if (user.UserName != newUserName)
                {
                    string patternUser = @"^(?=.{5,25}$)[a-zA-Z0-9_]+$";
                    if (!Regex.IsMatch(newUserName, patternUser, RegexOptions.None))
                    {
                        return BadRequest(new { message = "El nombre de usuario no tiene un formato válido." });
                    }

                    bool userExists = this.df.CreateDAOUser().GetUserByUsername(newUserName);
                    if (userExists)
                    {
                        return BadRequest(new { message = "El nombre de usuario ya está en uso." });
                    }

                    user.UserName = newUserName;
                }
            }

            if (!string.IsNullOrEmpty(request.email))
            {
                string newEmail = request.email.Trim().ToLower();

                if (user.Email != newEmail)
                {
                    string patternEmail = @"^[^\s@]+@[^\s@]+\.[^\s@]+$";
                    if (!Regex.IsMatch(newEmail, patternEmail, RegexOptions.IgnoreCase))
                    {
                        return BadRequest(new { message = "El formato del correo no es válido." });
                    }

                    bool emailExists = this.df.CreateDAOUser().GetUserByEmail(newEmail);
                    if (emailExists)
                    {
                        return BadRequest(new { message = "El correo electrónico ya está registrado." });
                    }

                    user.Email = newEmail;
                }
            }

            if (!string.IsNullOrEmpty(request.password))
            {
                string patternPass = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[\W_]).{8,}$";
                if (!Regex.IsMatch(request.password, patternPass, RegexOptions.None))
                {
                    return BadRequest(new { message = "La contraseña no cumple con los requisitos mínimos." });
                }

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


        [HttpGet("Suggestions/{idUserLogger}/{page}/{pageSize}")]
        public IActionResult suggestion([FromRoute] GetSugerenciasResquestDTO request)
        {
            // Obtener sugerencias desde el DAO
            var followingIds = this.df.CreateDAOFollowing().GetFollowedUserIds(request.idUserLogger);
            List<User> suggestion = this.df.CreateDAOUser()
                .GetSugerencias(request.idUserLogger, request.page, request.pageSize, followingIds);

            // Mapear a DTO de respuesta
            GetSugerenciasResponseDTO response = new GetSugerenciasResponseDTO
            {
                suggestions = suggestion.Select(u => new UserSuggestionDto
                {
                    id = u.Id,
                    userName = u.UserName,
                    avatar = u.Avatar.Url,
                    isFollowing = followingIds.Contains(u.Id)
                }).ToList()
            };

            return Ok(response);
        }

        [HttpGet]
        public IActionResult GetUsersBySearch([FromQuery] SearchUsersRequestDTO request)
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
