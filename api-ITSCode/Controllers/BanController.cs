using Microsoft.AspNetCore.Mvc;
using dao_library;
using entity_library;


namespace apiBan.Controllers
{
    [ApiController]
    [Route("Ban")]
    public class BanController : ControllerBase
    {
        private readonly ILogger<BanController> _logger;
        private DAOFactory df;

        public BanController(ILogger<BanController> logger, DAOFactory df)
        {
            _logger = logger;
            this.df = df;
        }



        [HttpGet("{id}")]
        public IActionResult getBan([FromRoute] GetBanRequestDTO request)
        {
            try
            {
                Ban? ban = this.df.CreateDAOBan().GetBanById(request.id);
                if (ban is null) return null;

                GetBanResponseDTO response = new GetBanResponseDTO
                {
                    userId = ban.User.Id,
                    reason = ban.Reason,
                    banDate = ban.BanDate.ToString("dd/MM/yyyy"),
                    unbanDate = ban.UnbanDate?.ToString("dd/MM/yyyy") ?? ""
                };

                return Ok(response);
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

        [HttpPost]
        public IActionResult CreateBan([FromBody] PostBanRequestDTO request)
        {
            try
            {
                User? user = this.df.CreateDAOUser().GetUser(request.userId);
                if (user is null) return NotFound();

                user.IsBanned = true;
                this.df.CreateDAOUser().UpdateUser(user);

                Ban ban = new Ban
                {
                    User = user,
                    Reason = request.reason,
                    BanDate = DateTime.Parse(request.banDate),
                    UnbanDate = string.IsNullOrEmpty(request.unbanDate)
                    ? (DateTime?)null
                    : DateTime.Parse(request.unbanDate)
                };

                this.df.CreateDAOBan().CreateBan(ban);

                PostBanResponseDTO response = new PostBanResponseDTO
                {
                    message = "Ban created successfully"
                };
                return Ok(response); 
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
        
        [HttpDelete]
        public IActionResult DeleteBan([FromBody] DeleteBanRequestDTO request)
        {
            try
            {    
                User? user = df.CreateDAOUser().GetUser(request.userId);

                Ban? ban = df.CreateDAOBan().GetBanByUserId(user);

                user.IsBanned = false;
                this.df.CreateDAOUser().UpdateUser(user);

                this.df.CreateDAOBan().DeleteBan(ban.Id);
                
                DeleteBanResponseDTO response = new DeleteBanResponseDTO
                {
                    message = "Ban deleted successfully",
                };
                return Ok(response);
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






