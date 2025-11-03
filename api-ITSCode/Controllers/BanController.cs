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
        public IActionResult getBan([FromQuery] GetBanRequestDTO request)
        {
            Ban ban = this.df.CreateDAOBan().GetBanById(request.id);
            if (ban == null) return null;

            GetBanResponseDTO response = new GetBanResponseDTO
            {
                userId = ban.User.Id,
                reason = ban.Reason,
                banDate = ban.BanDate.ToString("yyyy-MM-dd"),
                unbanDate = ban.UnbanDate?.ToString("yyyy-MM-dd") ?? ""
            };

            return Ok(response);
        }

        [HttpPost]
        public IActionResult CreateBan([FromBody] PostBanRequestDTO request)
        {
            User user = this.df.CreateDAOUser().GetUser(request.userId);
            if (user == null) return NotFound();

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
        
        [HttpDelete]
        public IActionResult DeleteBan([FromQuery] DeleteBanRequestDTO request)
        {
            Ban? ban = df.CreateDAOBan().GetBanById(request.id);
            if (ban == null)
            {
                return NotFound(new { message = "Ban not found" });
            }

            this.df.CreateDAOBan().DeleteBan(request.id);
            DeleteBanResponseDTO response = new DeleteBanResponseDTO
            {
                message = "Ban deleted successfully",
            };
            return Ok(response);
        }

    }

}






