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


        [HttpPost("create")]
        public IActionResult CreateUser([FromBody] PostUserRequestDTO dto)
        {
            Image avatar = new Image
            {
                Url = dto.URLAvatar
            };
            Role role = new Role
            {
                Id = dto.RoleId ?? 0
            };

            User user = new User
            {
                FullName = dto.FullName,
                UserName = dto.Username,
                Email = dto.Email,
                Password = dto.Password,
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


        [HttpGet("get")]
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

        [HttpPut("update")]
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

        [HttpDelete("delete")]
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


        [HttpPost]
        public IActionResult Login([FromBody] LoginRequestDTO request)
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
        
    //TODO: Carlos crea un controller que devuelva los usuairios buscados, que no sean seguidos por el loggeado, paginados, y que filtre tanto por nobre como por username
    }
}