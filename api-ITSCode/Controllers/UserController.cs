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
        public IActionResult CreateUser([FromBody] PostUserRequestDTO request)
        {
            Image avatar = new Image
            {
                Url = request.URLAvatar
            };

            Role? role = df.CreateDAORole().GetRoleById(request.RoleId ?? 0);

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


        [HttpGet("{userName}/{password}")]
        public IActionResult Login([FromQuery] LoginRequestDTO request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                string encryptedPassword = new User().encript(request.password);

                User user = this.df.CreateDAOUser().Login(request.userName, encryptedPassword);

                if (user == null)
                {
                    return Unauthorized(new { message = "Invalid username or password." });
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
            List<User> sugerencias = this.df.CreateDAOUser()
                .GetSugerencias(request.idUserLogger, request.page, request.pageSize);

            // Mapear a DTO de respuesta
            GetSugerenciasResponseDTO response = new GetSugerenciasResponseDTO
            {
                Sugerencias = sugerencias.Select(u => new UserSuggestionDto
                {
                    id = u.Id,
                    userName = u.UserName,
                    avatar = u.Avatar.Url
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

                List<User> users = this.df.CreateDAOUser().SearchUsers(request.searchTerm, request.idUserLogger, request.pageNumber, request.pageSize);
                var allUsers = users.Select(user => new GetUsersResponseDTO
                {
                    id = user.Id,
                    userAvatar = user.GetAvatar(),
                    userName = user.UserName
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