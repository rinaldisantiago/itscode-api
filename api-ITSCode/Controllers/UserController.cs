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



        [HttpGet("{idUser}")]
        public IActionResult Get(int idUser)
        {
            User? usuario = this.df.CreateDAOUser().GetUser(idUser);
            if (usuario == null)
            {
                return NotFound();
            }
            return Ok(usuario);
        }
        [HttpGet("all")]
        public IActionResult GetAll()
        {
            List<User> usuarios = this.df.CreateDAOUser().GetAll();
            if (usuarios == null )
            {
                return NotFound();
            }
            return Ok(usuarios);
        }

        
    }
}