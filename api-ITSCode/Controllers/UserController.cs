using Microsoft.AspNetCore.Mvc;
using dao_library;
using entity_library;


namespace apiUser.Controllers
{
    [ApiController]
    [Route("[controller]")]
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
        public IActionResult CreateUser([FromBody] PostUserRequestDTO dto)
        {
            User user = new User
            {
                FullName = dto.FullName,
                UserName = dto.Username,
                Email = dto.Email,
                Password = dto.Password,
                Role = dto.RoleId.HasValue ? this.df.CreateDAORole().GetRoleById(dto.RoleId.Value) : null,
                Avatar = this.df.CreateDAOImage().CreateImage(dto.URLAvatar)
            };

            this.df.CreateDAOUser().CreateUser(user);


            return Ok(new
            {
                message = "User created successfully",
                userName = user.FullName
            });
        }


        [HttpGet("")]
        public GetUserResponseDTO getUser([FromQuery] GetUserRequestDTO request)
        {
            User user = this.df.CreateDAOUser().GetUser(request.Id);
            if (user == null) return null;

            GetUserResponseDTO response = new GetUserResponseDTO
            {
                fullName = user.FullName,
                userName = user.UserName,
                email = user.Email,
                urlAvatar = user.GetAvatar
            };

            return response;
        }

        [HttpPut("")]
        public IActionResult UpdateUser(int id, [FromBody] PutUserRequestDTO request)
        {
            User user = this.df.CreateDAOUser().GetUser(id);
            if (user == null) return NotFound();
            user.FullName = request.fullName;
            user.UserName = request.userName;
            user.Email = request.email;
            user.Password = request.password;
            user.Avatar.Url = request.urlAvatar;


            User updateUser = this.df.CreateDAOUser().UpdateUser(user);
            
            PutUserResponseDTO response = new PutUserResponseDTO
            {
                fullName = updateUser.FullName,
                userName = updateUser.UserName,
                email = updateUser.Email,
                urlAvatar = updateUser.GetAvatar
            };

            return Ok(response);
        }

        [HttpDelete("")]
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
    }
}